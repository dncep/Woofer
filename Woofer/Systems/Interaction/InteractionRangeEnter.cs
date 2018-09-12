using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Interaction
{
    [Event("interaction_range_enter")]
    class InteractionRangeEnter : Event
    {
        public InteractionRangeEnter(Component sender) : base(sender)
        {
        }
    }
}
