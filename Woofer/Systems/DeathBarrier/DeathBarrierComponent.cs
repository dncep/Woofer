using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.DeathBarrier
{
    [Component("death_barrier")]
    class DeathBarrierComponent : Component
    {
        public double Y { get; private set; }

        public DeathBarrierComponent(double y) => Y = y;
    }
}
