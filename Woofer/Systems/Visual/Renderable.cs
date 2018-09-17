using System;
using System.Collections.Generic;
using System.Linq;
using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;

using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;

namespace WooferGame.Systems.Visual
{
    [Component("renderable")]
    class Renderable : Component
    {
        [PersistentProperty]
        public List<Sprite> Sprites { get; set; }

        public Renderable()
        {
            Sprites = new List<Sprite>();
        }

        public Renderable(string texture, Rectangle bounds)
        {
            Sprites = new List<Sprite>() { new Sprite(texture, bounds) };
        }

        public Renderable(params Sprite[] sprites)
        {
            Sprites = sprites.ToList();
        }

        public void AddSprite(Sprite sprite)
        {
            Sprites.Add(sprite);
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

                if(!new Rectangle(x, y, width, height)
                    .IntersectsWith(
                    new Rectangle(view.X - layer.GetSize().Width/2, view.Y - layer.GetSize().Height/2, layer.GetSize().Width, layer.GetSize().Height))) continue;
            
                x -= (int)Math.Floor(view.X);
                y -= (int)Math.Floor(view.Y);
            
                y *= -1;
                y -= height;
                
                x += layer.GetSize().Width / 2;
                y += layer.GetSize().Height / 2;


                System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)width, (int)height);

                if(sprite.Source is Rectangle source)
                {
                    layer.Draw(r.SpriteManager[sprite.Texture], drawingRect, sprite.Source.ToDrawing(), sprite.DrawMode);
                } else
                {
                    layer.Draw(r.SpriteManager[sprite.Texture], drawingRect, sprite.DrawMode);
                }
            }


        }
    }
}
