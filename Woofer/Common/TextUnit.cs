using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using WooferGame.Systems.Visual;
using Rectangle = EntityComponentSystem.Util.Rectangle;

namespace WooferGame.Common
{
    [PersistentObject]
    class TextUnit
    {
        private static readonly byte[] char_sizes =
        {
            8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            4, 3, 4, 6, 6, 7, 6, 4, 4, 4, 4, 6, 3, 6, 4, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 4, 4, 5, 5, 5, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 3, 6, 3, 6, 5,
            3, 5, 5, 5, 5, 5, 4, 5, 5, 2, 5, 5, 3, 6, 5, 6,
            5, 5, 5, 5, 4, 5, 6, 6, 6, 5, 5, 4, 4, 4, 7, 8
        };

        [PersistentProperty]
        public Sprite Icon;
        [PersistentProperty]
        public string Text;
        [PersistentProperty]
        public Color Color;

        public TextUnit() : this("")
        {
        }

        public TextUnit(string text) : this(null, text) { }

        public TextUnit(string text, Color color) : this(null, text, color) { }

        public TextUnit(Sprite icon, string text) : this(icon, text, Color.White) { }

        public TextUnit(Sprite icon, string text, Color color)
        {
            Icon = icon;
            Text = text;
            Color = color;
        }

        public void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r, DirectGraphicsContext<TSurface, TSource> layer, Point destination, int fontScale = 1)
        {
            var font = r.SpriteManager["font"];

            byte[] asciiBytes = Encoding.ASCII.GetBytes(Text);

            int destX = destination.X;

            if (Icon != null)
            {
                Rectangle iconDestination = new Rectangle(Icon.Destination);
                iconDestination.X += destination.X;
                iconDestination.Y += destination.Y;
                layer.Draw(r.SpriteManager[Icon.Texture], iconDestination.ToDrawing(), Icon.Source?.ToDrawing());

                destX += (int)iconDestination.Width + 4;
            }

            foreach (byte c in asciiBytes)
            {
                int srcX = (c % 16) * 8;
                int srcY = (c / 16) * 8;

                layer.Draw(font, new System.Drawing.Rectangle(destX, destination.Y, 8 * fontScale, 8 * fontScale), new System.Drawing.Rectangle(srcX, srcY, 8, 8), new DrawInfo() { Color = Color });
                destX += char_sizes[c] * fontScale;
            }
        }

        public System.Drawing.Size GetSize(int fontScale = 1)
        {
            int width = 0;
            int height = 8 * fontScale;
            if (Icon != null) height = Math.Max(height, (int)Icon.Destination.Height);

            byte[] asciiBytes = Encoding.ASCII.GetBytes(Text);

            foreach (byte c in asciiBytes)
            {
                width += char_sizes[c] * fontScale;
            }

            if (Icon != null)
            {
                width += (int)Icon.Destination.Width;
                width += 4;
            }

            return new System.Drawing.Size(width, height);
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
                width += char_sizes[c];
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
            int destY = 0;

            if (Icon != null)
            {
                Rectangle iconDestination = new Rectangle(Icon.Destination);
                iconDestination.X += destX;
                layer.Draw(r.SpriteManager[Icon.Texture], iconDestination.ToDrawing(), Icon.Source.ToDrawing());

                destX += (int)iconDestination.Width + 4;
                destY += (int)(iconDestination.Height / 2 - 4);
            }

            foreach (byte c in asciiBytes)
            {
                int srcX = (c % 16) * 8;
                int srcY = (c / 16) * 8;

                layer.Draw(font, new System.Drawing.Rectangle(destX, destY, 8, 8), new System.Drawing.Rectangle(srcX, srcY, 8, 8), new DrawInfo() { Color = Color });
                destX += (char_sizes[c]);
            }

            return surface;
        }
    }
}
