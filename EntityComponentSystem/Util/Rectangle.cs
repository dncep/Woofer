using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves;

namespace EntityComponentSystem.Util
{
    [PersistentObject]
    public class Rectangle
    {
        [PersistentProperty]
        public float X;
        [PersistentProperty]
        public float Y;
        [PersistentProperty]
        public float Width;
        [PersistentProperty]
        public float Height;

        public Vector2D Position => new Vector2D(X, Y);
        public Size Size => new Size(Width, Height);

        public float Left => X;
        public float Right => X + Width;
        public float Bottom => Y;
        public float Top => Y + Height;

        public Vector2D Center => new Vector2D(X + Width / 2, Y + Height / 2);

        public float Area => Width * Height;

        public Rectangle(Rectangle other) : this(other.X, other.Y, other.Width, other.Height)
        {
        }

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2D pos, float width, float height)
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

        public Rectangle() : this(0, 0, 0, 0)
        {
        }

        public static Rectangle operator +(Rectangle rect, Vector2D vect) => new Rectangle(rect.X + vect.X, rect.Y + vect.Y, rect.Width, rect.Height);
        public static Rectangle operator -(Rectangle rect, Vector2D vect) => new Rectangle(rect.X - vect.X, rect.Y - vect.Y, rect.Width, rect.Height);

        public override string ToString() => $"[x:{X}, y:{Y}, w:{Width}, h:{Height}]";
        public bool IntersectsWith(Rectangle other) => 
            this.Left < other.Right &&
            this.Right > other.Left &&
            this.Bottom < other.Top &&
            this.Top > other.Bottom;

        public bool Contains(Vector2D point)
        {
            return Left <= point.X && point.X <= Right && Bottom <= point.Y && point.Y <= Top;
        }

        public Rectangle Union(Rectangle other)
        {
            float x1 = Math.Min(this.Left, other.Left);
            float y1 = Math.Min(this.Bottom, other.Bottom);
            float x2 = Math.Max(this.Right, other.Right);
            float y2 = Math.Max(this.Top, other.Top);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
    }

    public static class RectangleConversion
    {
        public static System.Drawing.Rectangle ToDrawing(this Rectangle rect)
        {
            return new System.Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
