using System;

using EntityComponentSystem.Components;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;

using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;

namespace WooferGame.Systems.Visual
{
    [Component("renderable")]
    class Renderable : Component
    {
        public string Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Renderable(string texture)
        {
            this.Texture = texture;
        }

        public Renderable(string texture, Rectangle bounds)
        {
            this.Texture = texture;
            this.Bounds = bounds;
        }

        public void Render<TSurface, TSource>(DirectGraphicsContext<TSurface, TSource> layer, CameraView view, ScreenRenderer<TSurface, TSource> r)
        {
            IGameController controller = Woofer.Controller;

            Spatial spatial = Owner.Components.Get<Spatial>();
            float x = Bounds.X;
            float y = Bounds.Y;
            if (spatial != null)
            {
                x += ((float)(spatial.X));
                y += ((float)(spatial.Y));
            }
            float width = Bounds.Width;
            float height = Bounds.Height;
            
            x -= (int)Math.Floor(controller.ActiveScene.CurrentViewport.X);
            y -= (int)Math.Floor(controller.ActiveScene.CurrentViewport.Y);
            
            y *= -1;
            y -= height;

            x += layer.GetSize().Width / 2;
            y += layer.GetSize().Height / 2;


            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)width, (int)height);

            layer.Draw(r.SpriteManager[Texture], drawingRect);
        }
    }
}
