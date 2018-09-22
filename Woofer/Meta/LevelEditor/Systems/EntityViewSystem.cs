using System;
using System.Collections.Generic;
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
using WooferGame.Systems.Visual;
using Point = System.Drawing.Point;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("entity_view", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(EntitySelectEvent))]
    class EntityViewSystem : ComponentSystem
    {
        private bool Active = false;
        private long Selected;

        public override void Input()
        {

        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!Active) return;
            if (!Owner.Entities.ContainsId(Selected)) return;

            var layer = r.GetLayerGraphics("hi_res_overlay");

            Entity entity = Owner.Entities[Selected];

            int x = EditorRendering.SidebarX + EditorRendering.SidebarMargin + 4;
            int y = EditorRendering.SidebarMargin + 4;

            new TextUnit(new Sprite("editor", new Rectangle(0, 0, 16, 16), new Rectangle(0, 16, 16, 16)), entity.Name).Render(r, layer, new Point(x, y), 2);
            y += 20;
            new TextUnit("ID: " + Selected, System.Drawing.Color.DarkGray).Render(r, layer, new Point(x, y), 1);
            y += 20;


            foreach(Component component in entity.Components)
            {
                new TextUnit(new Sprite("editor", new Rectangle(0, 0, 16, 16), new Rectangle(0, 32, 16, 16)), component.ComponentName).Render(r, layer, new Point(x, y), 2);
                y += 20;
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is EntitySelectEvent s)
            {
                Selected = s.Entity.Id;
            } else if(e is ModalChangeEvent)
            {
                Active = true;
            } else if(e is BeginModalChangeEvent bmce)
            {
                bmce.SystemName = "entity_list";
                Active = false;
            }
        }
    }
}
