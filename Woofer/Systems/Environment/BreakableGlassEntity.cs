using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;

using WooferGame.Systems.Physics;
using WooferGame.Systems.Pulse;

namespace WooferGame.Systems.Environment
{
    class BreakableGlassEntity : Entity
    {
        public BreakableGlassEntity(Rectangle bounds)
        {
            Components.Add(new Spatial(bounds.X, bounds.Y));
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(new CollisionBox[] { new CollisionBox(0, 0, bounds.Width, bounds.Height) }));
            Components.Add(new PulseReceiver() { Offset = new Vector2D(bounds.Width / 2, bounds.Height / 2) });

            Components.Add(new BreakableGlassComponent());
        }
    }
}
