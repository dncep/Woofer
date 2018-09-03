using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Physics
{
    [Component("softbody")]
    class SoftBody : Component
    {
        public CollisionBox Bounds { get; private set; }
        public float Mass { get; set; }

        public SoftBody(CollisionBox bounds, float mass)
        {
            Bounds = bounds;
            Mass = mass;
        }
    }
}
