using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Physics
{
    [Component("rigidbody")]
    class RigidBody : Component
    {
        [PersistentProperty]
        public CollisionBox[] Bounds { get; set; }
        [HideInInspector]
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

        public RigidBody() : this(new CollisionBox[0])
        {
        }

        public RigidBody(params CollisionBox[] bounds) => Bounds = bounds;
    }
}
