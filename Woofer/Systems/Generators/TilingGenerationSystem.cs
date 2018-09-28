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
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Generators
{
    [ComponentSystem("tiling_generation", ProcessingCycles.Tick | ProcessingCycles.Render, ProcessingFlags.Pause),
        Watching(typeof(TilingGenerator))]
    class TilingGenerationSystem : ComponentSystem
    {
        public override void Tick()
        {

        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var layer = r.GetLayerGraphics("level");
            foreach(TilingGenerator gen in WatchedComponents)
            {
                if(gen.Preview)
                {
                    Renderable.Render(layer, Owner.CurrentViewport, r, gen.Sprites, gen.Owner.Components.Get<Spatial>());
                }
            }
        }
    }
}
