using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using WooferGame.Systems.Interaction;

namespace WooferGame.Systems.Variables
{
    [ComponentSystem("variables", ProcessingCycles.None),
        Listening(typeof(ActivationEvent)),
        Watching(typeof(CounterVariableComponent), typeof(VariableResetComponent))]
    class VariableSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event e)
        {
            if(e is ActivationEvent ae)
            {
                if(ae.Affected.Components.Get<VariableResetComponent>() is VariableResetComponent reset)
                {
                    Entity toReset = GetAffected(ae.Affected, reset.EntityToReset);
                    if(toReset != null)
                    {
                        if(toReset.Components.Get<CounterVariableComponent>() is CounterVariableComponent counterToReset)
                        {
                            counterToReset.Value = counterToReset.StartValue;
                        }
                    }
                }
                if(ae.Affected.Components.Get<CounterVariableComponent>() is CounterVariableComponent counter)
                {
                    counter.Value += counter.Increment;
                    if(counter.Value >= counter.EndValue)
                    {
                        if(counter.TriggerId != 0)
                        {
                            Entity trigger = Owner.Entities[counter.TriggerId];
                            if (trigger != null) Owner.Events.InvokeEvent(new ActivationEvent(counter, trigger, ae));
                        }
                        counter.Value = counter.StartValue;
                    }
                }
            }
        }

        private Entity GetAffected(Entity owner, long id)
        {
            if (id == 0) return owner;
            return Owner.Entities[id];
        }
    }
}
