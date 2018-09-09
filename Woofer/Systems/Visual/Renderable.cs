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
        public Sprite[] Sprites { get; set; }

        public Renderable()
        {
            Sprites = new Sprite[0];
        }

        public Renderable(string texture, Rectangle bounds)
        {
            Sprites = new Sprite[] { new Sprite(texture, bounds) };
        }

        public void Render<TSurface, TSource>(DirectGraphicsContext<TSurface, TSource> layer, CameraView view, ScreenRenderer<TSurface, TSource> r)
        {
            IGameController controller = Woofer.Controller;

            Spatial spatial = Owner.Components.Get<Spatial>();

            foreach(Sprite sprite in Sprites)
            {
                float x = (float)sprite.Destination.X;
                float y = (float)sprite.Destination.Y;
                if (spatial != null)
                {
                    x += ((float)(spatial.X));
                    y += ((float)(spatial.Y));
                }
                float width = (float)sprite.Destination.Width;
                float height = (float)sprite.Destination.Height;
            
                x -= (int)Math.Floor(controller.ActiveScene.CurrentViewport.X);
                y -= (int)Math.Floor(controller.ActiveScene.CurrentViewport.Y);
            
                y *= -1;
                y -= height;

                x += layer.GetSize().Width / 2;
                y += layer.GetSize().Height / 2;


                System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)width, (int)height);

                if(sprite.Source is Rectangle source)
                {
                    layer.Draw(r.SpriteManager[sprite.Texture], drawingRect, sprite.Source.ToDrawing());
                } else
                {
                    layer.Draw(r.SpriteManager[sprite.Texture], drawingRect);
                }
            }


        }
    }
}
