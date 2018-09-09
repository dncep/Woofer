using EntityComponentSystem.Components;

namespace WooferGame.Systems.Physics
{
    [Component("softbody")]
    class SoftBody : Component
    {
        public CollisionBox Bounds { get; private set; }
        public float Mass { get; set; }
        public bool Movable { get; set; } = true;

        public SoftBody(CollisionBox bounds, float mass)
        {
            Bounds = bounds;
            Mass = mass;
        }
    }
}
