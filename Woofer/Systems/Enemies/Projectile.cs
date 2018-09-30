using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.HealthSystems;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Enemies
{
    class Projectile : Entity
    {
        public Projectile(Vector2D pos, Vector2D velocity)
        {
            Components.Add(new Spatial(pos));
            Components.Add(new Physical() { Velocity = velocity });

            Components.Add(new LevelRenderable());
            Components.Add(new Renderable("brick", new Rectangle(-4, -4, 8, 8)));

            Components.Add(new SoftBody(new CollisionBox(-4, -4, 8, 8), 1f));

            Components.Add(new RemoveOnCollision());
            Components.Add(new RemoveOnBarrierComponent());
            Components.Add(new DamageOnContactComponent() { Damage = 1, Remove = true, Filter = DamageFilter.DamageAllies });
        }
    }
}
