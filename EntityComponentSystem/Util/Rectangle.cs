using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Util
{
    public class Rectangle
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public Vector2D Position => new Vector2D(X, Y);
        public Size Size => new Size(Width, Height);

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2D pos, double width, double height)
        {
            X = pos.X;
            Y = pos.Y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2D pos, Size size)
        {
            X = pos.X;
            Y = pos.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public static Rectangle operator +(Rectangle rect, Vector2D vect) => new Rectangle(rect.X + vect.X, rect.Y + vect.Y, rect.Width, rect.Height);
        public static Rectangle operator -(Rectangle rect, Vector2D vect) => new Rectangle(rect.X - vect.X, rect.Y - vect.Y, rect.Width, rect.Height);

        public override string ToString() => $"Rectangle[X={X},Y={Y},Width={Width},Height={Height}]";
    }

    public static class RectangleConversion
    {
        public static System.Drawing.Rectangle ToDrawing(this Rectangle rect)
        {
            return new System.Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
