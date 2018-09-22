using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using GameInterfaces.Controller;
using WooferGame.Common;

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

            new TextUnit(entity.Name).Render(r, layer, new System.Drawing.Rectangle(EditorRendering.SidebarX + EditorRendering.SidebarMargin + 4, EditorRendering.SidebarMargin + 4, EditorRendering.SidebarWidth - 4, 20), 2);
            new TextUnit("ID: " + Selected).Render(r, layer, new System.Drawing.Rectangle(EditorRendering.SidebarX + EditorRendering.SidebarMargin + 4, EditorRendering.SidebarMargin + 4 + 20, EditorRendering.SidebarWidth - 4, 10), 1);
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
