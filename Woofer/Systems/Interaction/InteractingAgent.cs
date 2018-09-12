using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using WooferGame.Input;

namespace WooferGame.Systems.Interaction
{
    [Component("interacting_agent")]
    class InteractingAgent : Component
    {
        public double MaxDistance;
        public InputTimeframe Input;

        public InteractingAgent(double maxDistance)
        {
            MaxDistance = maxDistance;
            Input = new InputTimeframe(2);
        }
    }
}
