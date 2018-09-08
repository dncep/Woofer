using System;
using System.Collections.Generic;

using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;

using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Visual
{
    class LevelSection : Entity
    {
        private List<CollisionBox> collision = new List<CollisionBox>();

        public LevelSection(string texture, Rectangle bounds)
        {
            Components.Add(new Spatial(bounds.X, bounds.Y));
            Components.Add(new Renderable(texture, new Rectangle(0, 0, bounds.Width, bounds.Height)));
            Components.Add(new LevelRenderable());
        }

        protected void AddCollision(CollisionBox box)
        {
            collision.Add(box);
        }

        protected void AddSlope(Vector2D from, Vector2D to, double friction)
        {
            double minX = Math.Min(from.X, to.X);
            double width = Math.Abs(from.X - to.X);
            double minY = Math.Min(from.Y, to.Y);
            double height = Math.Abs(from.Y - to.Y);

            double stepWidth;
            double stepHeight;

            if(width >= height)
            {
                stepWidth = width / height;
                stepHeight = 1;
            } else
            {
                stepWidth = 1;
                stepHeight = height / width;
            }
            if (stepWidth <= 0) throw new ArgumentException("Width cannot be zero");
            if (stepHeight<= 0) throw new ArgumentException("Height cannot be zero");
            
            double y = (to.Y > from.Y) ? minY + height - stepHeight : minY;

            for (double x = minX; x < minX + width; x += stepWidth)
            {
                AddCollision(new CollisionBox(x, y, stepWidth, stepHeight)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, friction, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                });
                AddCollision(new CollisionBox(x + (to.Y > from.Y ? -stepWidth : +stepWidth), y, stepWidth, stepHeight));
                y += (to.Y > from.Y) ? -stepHeight : stepHeight;
            }
        }

        protected void FinalizeCollision()
        {
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(collision.ToArray()));
        }
    }
}
