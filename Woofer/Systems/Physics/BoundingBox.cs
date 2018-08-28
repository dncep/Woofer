using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace Woofer.Systems.Physics
{
    public class BoundingBox : IEquatable<BoundingBox>
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public double Left => X;
        public double Right => X + Width;
        public double Bottom => Y;
        public double Top => Y + Height;

        public double Area => Width * Height;
        public double Perimeter => 2 * (Width + Height);

        public BoundingBox(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingBox Offset(Vector2D delta)
        {
            return new BoundingBox(X + delta.X, Y + delta.Y, Width, Height);
        }

        public bool IntersectsWith(BoundingBox other)
        {
            return this.Left < other.Right &&
                    this.Right > other.Left &&
                    this.Bottom < other.Top &&
                    this.Top > other.Bottom;
        }

        public BoundingBox Intersect(BoundingBox rectangle)
        {
            if (!IntersectsWith(rectangle)) return null;
            double x1 = Math.Max(this.Left, rectangle.Left);
            double y1 = Math.Max(this.Bottom, rectangle.Bottom);
            double x2 = Math.Min(this.Right, rectangle.Right);
            double y2 = Math.Min(this.Top, rectangle.Top);
            return new BoundingBox(x1, y1, x2 - x1, y2 - y1);
        }

        public Vector2D[] GetVertices()
        {
            Vector2D[] vertices = new Vector2D[4];
            vertices[0] = new Vector2D(Left, Top);
            vertices[1] = new Vector2D(Right, Top);
            vertices[2] = new Vector2D(Right, Bottom);
            vertices[3] = new Vector2D(Left, Bottom);
            return vertices;
        }

        public FreeVector2D[] GetSides()
        {
            FreeVector2D[] sides = new FreeVector2D[4];
            Vector2D[] vertices = GetVertices();
            sides[0] = new FreeVector2D(vertices[0], vertices[1]);
            sides[1] = new FreeVector2D(vertices[1], vertices[2]);
            sides[2] = new FreeVector2D(vertices[2], vertices[3]);
            sides[3] = new FreeVector2D(vertices[3], vertices[0]);
            return sides;
        }

        public override string ToString() => $"BoundingBox[X={X},Y={Y},Width={Width},Height={Height}]";

        public override bool Equals(object obj) => Equals(obj as BoundingBox);
        public bool Equals(BoundingBox other) => other != null && X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

        public override int GetHashCode()
        {
            var hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}
