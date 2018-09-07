using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected void FinalizeCollision()
        {
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(collision.ToArray()));
        }
    }
}
