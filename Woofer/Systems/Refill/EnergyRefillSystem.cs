using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;

namespace WooferGame.Systems.Refill
{
    [ComponentSystem("energy_refill"),
        Listening(typeof(SoftCollisionEvent))]
    class EnergyRefillSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event evt)
        {
            if(evt is SoftCollisionEvent ce &&
                ce.Sender.Owner.Components.Has<EnergyRefillComponent>() &&
                ce.Victim.Components.Get<PulseAbility>() is PulseAbility pulse)
            {
                pulse.EnergyMeter = pulse.MaxEnergy;
            }
        }
    }
}
