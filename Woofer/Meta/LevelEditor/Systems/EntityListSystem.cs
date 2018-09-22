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
using GameInterfaces.Input;
using WooferGame.Common;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor.Systems.EntityOutlines;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("entity_list", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause)]
    class EntityListSystem : ComponentSystem
    {
        private bool ModalActive = false;
        private bool ModalVisible = true;
        private int SelectedIndex = 0;
        private int StartOffset = 0;

        private int AmountVisible = 0;

        public override void Input()
        {
            if (!ModalActive) return;
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D movement = inputMap.Movement;

            Editor.CycleTimeframe.RegisterState((movement).Magnitude > 1e-5 ? ButtonState.Pressed : ButtonState.Released);

            if(movement.Magnitude > 1e-5 && Editor.CycleTimeframe.Execute())
            {
                Owner.Events.InvokeEvent(new ClearEntityOutlines(null));
                if (movement.Y > 0)
                {
                    if (SelectedIndex - 1 >= 0) SelectedIndex--;
                }
                else if (movement.Y < 0)
                {
                    if (SelectedIndex + 1 < Owner.Entities.Count) SelectedIndex++;
                }
                Owner.Events.InvokeEvent(new BeginEntityOutline(Owner.Entities.ToList()[SelectedIndex], null));
                if(SelectedIndex < StartOffset)
                {
                    StartOffset = SelectedIndex;
                }
                if(SelectedIndex > StartOffset + AmountVisible)
                {
                    StartOffset = SelectedIndex - AmountVisible;
                }
            }

            Editor.SelectTimeframe.RegisterState(inputMap.Jump);
            if(inputMap.Jump.IsPressed() && Editor.SelectTimeframe.Execute())
            {
                Owner.Events.InvokeEvent(new EntitySelectEvent(Owner.Entities.ToList()[SelectedIndex], null));
                Owner.Events.InvokeEvent(new ForceModalChangeEvent("entity_view", null));
                ModalActive = false;
                ModalVisible = false;
            }
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!ModalVisible) return;
            SelectedIndex = Math.Max(0, Math.Min(SelectedIndex, Owner.Entities.Count-1));
            StartOffset = Math.Max(0, Math.Min(StartOffset, Owner.Entities.Count-1));
            var layer = r.GetLayerGraphics("hi_res_overlay");

            int y = 10;
            int x = EditorRendering.SidebarX + EditorRendering.SidebarMargin;

            List<Entity> entities = Owner.Entities.ToList();
            int index = StartOffset;
            for(; index < entities.Count; index++)
            {
                Entity entity = entities[index];
                if(index == SelectedIndex)
                {
                    layer.FillRect(new System.Drawing.Rectangle(x, y, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 20), ModalActive ? Color.CornflowerBlue : Color.FromArgb(63, 63, 70));
                }
                TextUnit label = new TextUnit(entity.Name);
                label.Render<TSurface, TSource>(r, layer, new Point(x, y+2), 2);
                y += 20;
                if (y > 720 - 16) break;
            }
            AmountVisible = index - StartOffset - 1;
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is BeginModalChangeEvent bmce)
            {
                bmce.SystemName = "editor_cursor";
                ModalActive = false;
                ModalVisible = true;
            }
            else if (e is ModalChangeEvent)
            {
                ModalActive = true;
                ModalVisible = true;
            }
        }
    }
}
