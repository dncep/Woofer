using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;

namespace WooferGame.Systems.DeathBarrier
{
    class DeathBarrier : Entity
    {
        public DeathBarrier(double y)
        {
            this.Components.Add(new DeathBarrierComponent(y));
        }
    }
}
