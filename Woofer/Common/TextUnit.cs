using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using WooferGame.Systems.Visual;

namespace WooferGame.Common
{
    class TextUnit
    {
        private static readonly byte[] char_sizes =
        {
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            5, 4, 5, 7, 7, 8, 7, 5, 5, 5, 5, 7, 4, 7, 5, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 6, 6, 6, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 4, 7, 4, 7, 7,
            4, 6, 6, 6, 6, 6, 5, 6, 6, 3, 6, 5, 5, 7, 6, 7,
            6, 6, 6, 6, 5, 6, 7, 7, 7, 6, 6, 5, 5, 5, 8, 8
        };
        
        public Sprite Icon;
        public string Text;

        public TextUnit(string text) : this(null, text) { }

        public TextUnit(Sprite icon, string text)
        {
            Icon = icon;
            Text = text;
        }

        public TSurface Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            int width = 0;
            int height = 8;
            if (Icon != null) height = Math.Max(8, (int)Icon.Destination.Height);
            var font = r.SpriteManager["font"];

            byte[] asciiBytes = Encoding.ASCII.GetBytes(Text);

            foreach (byte c in asciiBytes)
            {
                width += char_sizes[c] - 1;
            }

            if (Icon != null)
            {
                width += (int)Icon.Destination.Width;
                width += 4;
            }

            if (width == 0) return r.GraphicsContext.CreateTarget(1, 1);

            TSurface surface = r.GraphicsContext.CreateTarget(width, height);
            DirectGraphicsContext<TSurface, TSource> layer = new DirectGraphicsContext<TSurface, TSource>(surface, r.GraphicsContext);

            int destX = 0;

            if (Icon != null)
            {
                Rectangle iconDestination = new Rectangle(Icon.Destination);
                iconDestination.X += destX;
                layer.Draw(r.SpriteManager[Icon.Texture], iconDestination.ToDrawing(), Icon.Source.ToDrawing());

                destX += (int)iconDestination.Width + 4;
            }

            foreach (byte c in asciiBytes)
            {
                int srcX = (c % 16) * 8;
                int srcY = (c / 16) * 8;

                layer.Draw(font, new System.Drawing.Rectangle(destX, 0, 8, 8), new System.Drawing.Rectangle(srcX, srcY, 8, 8));
                destX += (char_sizes[c] - 1);
            }

            return surface;
        }
    }
}
