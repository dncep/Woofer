using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Interfaces.Input;
using GameInterfaces.Input;
using WooferGame.Input;

namespace WooferGame.Systems.Interaction
{
    [ComponentSystem("interaction", ProcessingCycles.Input),
        Watching(typeof(InteractingAgent), typeof(Interactable))]
    class InteractionSystem : ComponentSystem
    {
        public override void Input()
        {
            IEnumerable<InteractingAgent> agents = WatchedComponents.Where(c => c is InteractingAgent && c.Owner.Active).Select(c => c as InteractingAgent);

            foreach(Component rawC in WatchedComponents)
            {
                if (!rawC.Owner.Active) continue;
                if (rawC is Interactable interactable)
                {
                    InteractingAgent currentAgent = null;
                    bool inRange = false;
                    foreach(InteractingAgent agent in agents)
                    {
                        if ((agent.Owner.Components.Get<Spatial>().Position - interactable.Owner.Components.Get<Spatial>().Position).Magnitude <= agent.MaxDistance)
                        {
                            currentAgent = agent;
                            inRange = true;
                            break;
                        }
                    }
                    if(inRange != interactable.InRange)
                    {
                        Owner.Events.InvokeEvent(inRange ?
                            (Event) new InteractionRangeEnter(currentAgent, interactable) :
                            (Event) new InteractionRangeExit(null));
                        interactable.InRange = inRange;
                    }

                    if(inRange)
                    {
                        ButtonInput interact = Woofer.Controller.InputManager.ActiveInputMap.Interact;
                        
                        if(interact.Consume())
                        {
                            Entity sendTo = interactable.EntityToActivate != 0 ? Owner.Entities[interactable.EntityToActivate] : interactable.Owner;
                            Owner.Events.InvokeEvent(new ActivationEvent(currentAgent, sendTo, null));
                        }
                    }
                }
            }
        }
    }
}
