using System;
using System.Collections.Generic;
using System.Drawing;
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
using GameInterfaces.Input;
using WooferGame.Common;
using WooferGame.Input;
using WooferGame.Systems.Visual;
using Point = System.Drawing.Point;
using Rectangle = EntityComponentSystem.Util.Rectangle;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("entity_view", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(EntitySelectEvent))]
    class EntityViewSystem : ComponentSystem
    {
        private bool ModalActive = false;
        private long Selected;

        private int SelectedComponentIndex = 0;
        private int SelectedPropertyIndex = 0;
        private bool ComponentLocked = false;


        public override void Input()
        {
            if (!ModalActive) return;
            if (!Owner.Entities.ContainsId(Selected)) return;
            Entity entity = Owner.Entities[Selected];

            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D movement = inputMap.Movement;

            Editor.CycleTimeframe.RegisterState((movement).Magnitude > 1e-5 ? ButtonState.Pressed : ButtonState.Released);

            if (movement.Magnitude > 1e-5 && Editor.CycleTimeframe.Execute())
            {
                if(!ComponentLocked)
                {
                    if (movement.Y > 0)
                    {
                        if (SelectedComponentIndex - 1 >= -1) SelectedComponentIndex--;
                    }
                    else if (movement.Y < 0)
                    {
                        if (SelectedComponentIndex + 1 < entity.Components.Count) SelectedComponentIndex++;
                    }
                }

                /*if (SelectedComponentIndex < StartOffset)
                {
                    StartOffset = SelectedComponentIndex;
                }
                if (SelectedComponentIndex > StartOffset + AmountVisible)
                {
                    StartOffset = SelectedComponentIndex - AmountVisible;
                }*/
            }

            Editor.SelectTimeframe.RegisterState(inputMap.Jump);
            if (inputMap.Jump.IsPressed() && Editor.SelectTimeframe.Execute())
            {
                if(SelectedComponentIndex == -1)
                {
                    entity.Active = !entity.Active;
                }
                else if (!ComponentLocked)
                {
                    ComponentLocked = true;
                }
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!ModalActive) return;
            if (!Owner.Entities.ContainsId(Selected)) return;

            var layer = r.GetLayerGraphics("hi_res_overlay");

            Entity entity = Owner.Entities[Selected];

            int x = EditorRendering.SidebarX + EditorRendering.SidebarMargin + 4;
            int y = EditorRendering.SidebarMargin + 4;

            new TextUnit(new Sprite("editor", new Rectangle(0, 0, 16, 16), new Rectangle(0, 16, 16, 16)), entity.Name).Render(r, layer, new Point(x, y), 2);
            y += 20;
            new TextUnit("ID: " + Selected, System.Drawing.Color.DarkGray).Render(r, layer, new Point(x, y), 1);
            y += 22;

            if(SelectedComponentIndex == -1) layer.FillRect(new System.Drawing.Rectangle(x - 4, y - 2, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 20), Color.CornflowerBlue);
            new TextUnit(new Sprite("editor", new Rectangle(0, 0, 16, 16), new Rectangle(entity.Active ? 8 : 0, 48, 8, 8)), "Active").Render(r, layer, new Point(x, y), 2);

            y += 20;

            layer.FillRect(new System.Drawing.Rectangle(x-4, y, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 3), Color.FromArgb(45, 45, 48));
            y += 8;

            int index = 0;
            foreach(Component component in entity.Components)
            {
                if(index == SelectedComponentIndex)
                {
                    layer.FillRect(new System.Drawing.Rectangle(x - 4, y - 2, EditorRendering.SidebarWidth - 2 * EditorRendering.SidebarMargin, 20), ComponentLocked ? Color.FromArgb(63, 63, 70) : Color.CornflowerBlue);
                }
                new TextUnit(
                    //new Sprite("editor", new Rectangle(0, 0, 16, 16), new Rectangle(0, 32, 16, 16)), 
                    component.ComponentName)
                    .Render(r, layer, new Point(x, y), 2);
                y += 24;

                PropertyInfo[] props = component.GetType().GetProperties();
                
                foreach(PropertyInfo property in props)
                {
                    if (property.DeclaringType == typeof(Component)) continue;
                    if (property.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                    new TextUnit(property.Name + ": " + property.GetValue(component).ToString(), ComponentLocked && SelectedComponentIndex == index ? Color.White : Color.Gray).Render(r, layer, new Point(x+8, y), 1);
                    y += 16;
                }

                foreach(FieldInfo field in component.GetType().GetFields())
                {
                    if (field.DeclaringType == typeof(Component)) continue;
                    if (field.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                    new TextUnit(field.Name + ": " + field.GetValue(component).ToString(), ComponentLocked && SelectedComponentIndex == index ? Color.White : Color.Gray).Render(r, layer, new Point(x + 8, y), 1);
                    y += 16;
                }

                y += 16;

                index++;
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is EntitySelectEvent s)
            {
                Selected = s.Entity.Id;
                SelectedComponentIndex = -1;
                ComponentLocked = false;
            } else if(e is ModalChangeEvent)
            {
                ModalActive = true;
            } else if(e is BeginModalChangeEvent bmce)
            {
                if (ComponentLocked) ComponentLocked = false;
                else
                {
                    bmce.SystemName = "entity_list";
                    ModalActive = false;
                }
            }
        }
    }
}
