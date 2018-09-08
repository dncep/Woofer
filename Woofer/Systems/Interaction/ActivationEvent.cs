using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Interaction
{
    [Event("activation")]
    class ActivationEvent : Event
    {
        public Entity Affected;

        public ActivationEvent(Component sender, Entity affected) : base(sender)
        {
            this.Affected = affected;
        }
    }
}
