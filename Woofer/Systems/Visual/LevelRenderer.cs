using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Scenes;
using GameInterfaces.Controller;
using WooferGame.Test_Data;

namespace WooferGame.Systems.Visual
{
    [ComponentSystem("level_renderer")]
    public class LevelRenderer : ComponentSystem
    {
        public LevelRenderer()
        {
            Watching = new string[] { Component.IdentifierOf<LevelRenderable>() };
            RenderProcessing = true;
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            /*var bg = r.GetLayerGraphics("background");
            bg.Clear(System.Drawing.Color.FromArgb(32, 255, 0, 255));
            bg.Complete();*/

            var layer = r.GetLayerGraphics("level");

            CameraView view = Owner.CurrentViewport;

            foreach (LevelRenderable tile in WatchedComponents)
            {
                tile.Owner.Components.Get<Renderable>().Render(layer, view, r);
            }

            layer.Complete();
        }
    }
}
