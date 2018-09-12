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
        private List<Entity> queuedEntities = new List<Entity>();

        public LevelSection(string texture, Rectangle bounds)
        {
            Components.Add(new Spatial(bounds.X, bounds.Y));
            Components.Add(new Renderable(texture, new Rectangle(0, 0, bounds.Width, bounds.Height)));
            Components.Add(new LevelRenderable());
        }

        protected void AddCollision(CollisionBox box)
        {
            AddCollision(CoordinateMode.Real, box);
        }

        protected void AddCollision(CoordinateMode mode, CollisionBox box)
        {
            if(mode == CoordinateMode.Grid)
            {
                box.X *= 16;
                box.Y *= 16;
                box.Width *= 16;
                box.Height *= 16;
            }
            collision.Add(box);
        }

        protected void QueueEntity(Entity entity)
        {
            queuedEntities.Add(entity);
        }

        protected void FinalizeCollision()
        {
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(collision.ToArray()));
            collision.Clear();
            collision = null;
        }

        public override void Initialize()
        {
            foreach(Entity entity in queuedEntities)
            {
                if(entity.Components.Get<Spatial>() is Spatial sp)
                {
                    sp.Position += this.Components.Get<Spatial>().Position;
                }
                Owner.Entities.Add(entity);
            }
            queuedEntities.Clear();
            queuedEntities = null;
        }
    }

    enum CoordinateMode
    {
        Real,
        Grid
    }
}
