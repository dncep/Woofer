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
using GameInterfaces.Controller;
using WooferGame.Common;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("EntityListSystem", ProcessingCycles.Render, ProcessingFlags.Pause)]
    class EntityListSystem : ComponentSystem
    {
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var layer = r.GetLayerGraphics("hi_res_overlay");

            int y = 0;

            foreach(Entity entity in Owner.Entities)
            {
                if (entity.Components.Has<EditorMarker>()) continue;
                TextUnit label = new TextUnit(entity.Name);
                label.Render<TSurface, TSource>(r, layer, new Rectangle(0, y, 100, 10));
                y += 10;
            }
        }
    }
}
