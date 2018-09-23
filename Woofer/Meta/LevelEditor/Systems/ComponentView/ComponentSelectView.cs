using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using static WooferGame.Meta.LevelEditor.Systems.ComponentView.StartComponentSelectEvent;

namespace WooferGame.Meta.LevelEditor.Systems.ComponentView
{
    [ComponentSystem("component_select", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(StartComponentSelectEvent))]
    class ComponentSelectViewSystem : ComponentSystem
    {
        private bool ModalActive = false;
        private int SelectedIndex = 0;
        private int StartOffset = 0;

        private int AmountVisible = 0;

        public override bool ShouldSave => false;

        private List<string> ComponentNames;

        private OnSubmit Callback = null;
        private string SwitchTo = null;

        public ComponentSelectViewSystem()
        {
            ComponentNames = Component.GetAllIdentifiers();
        }

        public override void Input()
        {
            if (!ModalActive) return;
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D movement = inputMap.Movement;

            if (movement.Magnitude > 1e-5 && Editor.MoveTimeframe.Execute())
            {
                if (movement.Y > 0)
                {
                    if (SelectedIndex - 1 >= 0) SelectedIndex--;
                }
                else if (movement.Y < 0)
                {
                    if (SelectedIndex + 1 < ComponentNames.Count) SelectedIndex++;
                }
                if (SelectedIndex < StartOffset)
                {
                    StartOffset = SelectedIndex;
                }
                if (SelectedIndex > StartOffset + AmountVisible)
                {
                    StartOffset = SelectedIndex - AmountVisible;
                }
            }

            if (Editor.SelectTimeframe.Execute())
            {
                Submit();
            }
        }

        private void Submit()
        {
            Callback(ComponentNames[SelectedIndex]);
            Callback = null;
            ModalActive = false;
            Owner.Events.InvokeEvent(new ForceModalChangeEvent(SwitchTo, null));
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!ModalActive) return;
            SelectedIndex = Math.Max(0, Math.Min(SelectedIndex, Owner.Entities.Count - 1));
            StartOffset = Math.Max(0, Math.Min(StartOffset, Owner.Entities.Count - 1));
            var layer = r.GetLayerGraphics("hi_res_overlay");

            int y = 10;
            int x = EditorRendering.SidebarX + EditorRendering.SidebarMargin;
            
            int index = StartOffset;
            for (; index < ComponentNames.Count; index++)
            {
                string identifier = ComponentNames[index];
                if (index == SelectedIndex)
                {
                    layer.FillRect(new System.Drawing.Rectangle(x, y, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 20), ModalActive ? Color.CornflowerBlue : Color.FromArgb(63, 63, 70));
                }
                TextUnit label = new TextUnit(identifier);
                label.Render<TSurface, TSource>(r, layer, new Point(x, y + 2), 2);
                y += 20;
                if (y > 720 - 16) break;
            }
            AmountVisible = index - StartOffset - 1;
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is BeginModalChangeEvent bmce)
            {
                bmce.SystemName = SwitchTo;
                ModalActive = false;
            }
            else if (e is ModalChangeEvent change)
            {
                SwitchTo = change.From;
                ModalActive = true;
            } else if(e is StartComponentSelectEvent start)
            {
                Callback = start.Callback;
            }
        }
    }

    [Event("start_component_select")]
    public class StartComponentSelectEvent : Event
    {
        public OnSubmit Callback;
        
        public StartComponentSelectEvent(OnSubmit callback, Component sender) : base(sender)
        {
            Callback = callback;
        }

        public delegate void OnSubmit(string value);
    }
}
