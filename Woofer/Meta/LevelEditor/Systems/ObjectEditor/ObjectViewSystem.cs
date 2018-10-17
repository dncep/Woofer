﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using WooferGame.Common;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor.Systems.EntityView;
using WooferGame.Systems.Visual;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace WooferGame.Meta.LevelEditor.Systems.ObjectEditor
{
    [ComponentSystem("object_editor", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(StartObjectEditEvent))]
    class ObjectViewSystem : ComponentSystem
    {
        private bool ModalActive = false;
        private bool ModalVisible = false;

        private int SelectedPropertyIndex = 0;
        private List<IMemberSummary> Members = new List<IMemberSummary>();

        private string Title;
        private object Object;

        private string ReturnTo;

        public override bool ShouldSave => false;

        public override void Input()
        {
            if (!ModalActive) return;
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D movement = inputMap.Movement;

            if (movement.Magnitude > 1e-5 && Editor.MoveTimeframe.Execute())
            {
                if (movement.Y > 0)
                {
                    if (SelectedPropertyIndex - 1 >= 0) SelectedPropertyIndex--;
                }
                else if (movement.Y < 0)
                {
                    if (SelectedPropertyIndex + 1 < Members.Count) SelectedPropertyIndex++;
                }
            }

            if (inputMap.Jump.Consume())
            {
                IMemberSummary member = Members.ElementAt(SelectedPropertyIndex);
                if (member.CanSet)
                {
                    bool modalNeedsChange = member.TriggerEdit(inputMap.Interact.Pressed);
                    if (modalNeedsChange)
                    {
                        ModalActive = false;
                    }
                }
            }
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!ModalVisible) return;

            var layer = r.GetLayerGraphics("hi_res_overlay");

            System.Drawing.Rectangle sidebar = new System.Drawing.Rectangle(EditorRendering.SidebarX, 0, EditorRendering.SidebarWidth, 720);

            layer.FillRect(sidebar, Color.FromArgb(45, 45, 48));
            layer.FillRect(new System.Drawing.Rectangle(sidebar.X + EditorRendering.SidebarMargin, sidebar.Y + EditorRendering.SidebarMargin, sidebar.Width - 2 * EditorRendering.SidebarMargin, sidebar.Height - 2 * EditorRendering.SidebarMargin), Color.FromArgb(37, 37, 38));

            int x = EditorRendering.SidebarX + 2 * EditorRendering.SidebarMargin;
            int y = EditorRendering.SidebarMargin + 4;

            new TextUnit(EditorMenuSystem.CogIcon, Title).Render(r, layer, new Point(x, y), 2);
            y += 20;

            layer.FillRect(new System.Drawing.Rectangle(x - 2 * EditorRendering.SidebarMargin, y, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 3), Color.FromArgb(45, 45, 48));
            y += 8;

            int index = 0;

            foreach (IMemberSummary member in Members)
            {
                TextUnit label = member.Label;
                label.Color = SelectedPropertyIndex == index ? Color.CornflowerBlue : Color.White;
                label.Render(r, layer, new Point(x + 8, y), 1);
                y += 16;
                index++;
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is StartObjectEditEvent s)
            {
                SelectedPropertyIndex = 0;
                Members.Clear();
                Object = s.Object;
                Title = s.Title;
                ReturnTo = s.ReturnTo;
                foreach (FieldInfo field in Object.GetType().GetFields())
                {
                    Members.Add(new FieldSummary(Owner, field, Object));
                }
                foreach (PropertyInfo property in Object.GetType().GetProperties())
                {
                    Members.Add(new PropertySummary(Owner, property, Object));
                }
                if(ReturnTo == null) ReturnTo = (Owner.Systems[ComponentSystem.IdentifierOf<ModalFocusSystem>()] as ModalFocusSystem)?.CurrentSystem ?? "editor_menu";
            }
            else if (e is ModalChangeEvent)
            {
                ModalActive = true;
                ModalVisible = true;
            }
            else if (e is BeginModalChangeEvent bmce)
            {
                bmce.SystemName = ReturnTo;
                ModalActive = false;
                ModalVisible = false;
            }
        }
    }

    [Event("object_edit_select")]
    class StartObjectEditEvent : Event
    {
        public object Object;
        public string Title;
        public string ReturnTo;

        public StartObjectEditEvent(object obj, string title, string returnTo) : base(null)
        {
            Object = obj;
            Title = title;
            ReturnTo = returnTo;
        }
    }
}
