using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Timer
{
    [Component("timer")]
    class TimerComponent : Component
    {
        public double TimeElapsed = 0;
        public double Period;

        public TimerComponent(double period)
        {
            this.Period = period;
        }
    }
}
