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
    [ComponentSystem("EntityListSystem", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(BeginModalChangeEvent))]
    class EntityListSystem : ComponentSystem
    {
        private bool InputActive = false;
        private int SelectedIndex = 0;
        private int StartOffset = 0;

        private int AmountVisible = 0;

        private InputRepeatingTimeframe CycleTimeframe = new InputRepeatingTimeframe(15, 3);

        public override void Input()
        {
            if (!InputActive) return;
            Vector2D movement = Woofer.Controller.InputManager.ActiveInputMap.Movement;

            CycleTimeframe.RegisterState((movement).Magnitude > 1e-5 ? ButtonState.Pressed : ButtonState.Released);

            if(movement.Magnitude > 1e-5 && CycleTimeframe.Execute())
            {
                Owner.Events.InvokeEvent(new ClearEntityOutlines(null));
                if (movement.Y > 0)
                {
                    if (SelectedIndex - 1 >= 0) SelectedIndex--;
                    else return;
                }
                else if (movement.Y < 0)
                {
                    if (SelectedIndex + 1 < Owner.Entities.Count) SelectedIndex++;
                    else return;
                }
                else return;
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
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
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
                    layer.FillRect(new System.Drawing.Rectangle(x, y, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 20), InputActive ? Color.CornflowerBlue : Color.FromArgb(63, 63, 70));
                }
                TextUnit label = new TextUnit(entity.Name);
                label.Render<TSurface, TSource>(r, layer, new System.Drawing.Rectangle(x, y+2, 100, 20), 2);
                y += 20;
                if (y > 720 - 16) break;
            }
            AmountVisible = index - StartOffset - 1;
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is BeginModalChangeEvent bmce)
            {
                if (!InputActive) bmce.SystemName = this.SystemName;
                InputActive = false;
            }
            else if (e is ModalChangeEvent)
            {
                InputActive = true;
            }
        }
    }
}
