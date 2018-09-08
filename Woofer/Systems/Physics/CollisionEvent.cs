using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Physics
{
    [Event("collision")]
    class CollisionEvent : Event
    {
        public Entity Victim { get; private set; }
        public Vector2D Normal { get; private set; }

        public CollisionEvent(Component sender, Entity victim, Vector2D normal) : base(sender)
        {
            this.Victim = victim;
            this.Normal = normal;
        }
    }
}
