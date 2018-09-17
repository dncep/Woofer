using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;

namespace WooferGame.Systems.Physics
{
    [Component("softbody")]
    class SoftBody : Component
    {
        [PersistentProperty]
        public CollisionBox Bounds { get; private set; }
        [PersistentProperty]
        public float Mass { get; set; }
        [PersistentProperty]
        public bool Movable { get; set; } = true;

        public SoftBody()
        {
        }

        public SoftBody(CollisionBox bounds, float mass)
        {
            Bounds = bounds;
            Mass = mass;
        }
    }
}
