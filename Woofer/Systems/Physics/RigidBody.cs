using EntityComponentSystem.Components;

namespace WooferGame.Systems.Physics
{
    [Component("rigidbody")]
    class RigidBody : Component
    {
        public CollisionBox[] Bounds { get; set; }
        public CollisionBox UnionBounds {
            get
            {
                if (Bounds.Length == 0) return new CollisionBox(0, 0, 0, 0);
                else if (Bounds.Length == 1) return Bounds[0];
                else
                {
                    CollisionBox union = Bounds[0];
                    for(int i = 0; i < Bounds.Length; i++)
                    {
                        union = union.Union(Bounds[i]);
                    }
                    return union;
                }
            }
        }
        public RigidBody(params CollisionBox[] bounds) => Bounds = bounds;
    }
}
