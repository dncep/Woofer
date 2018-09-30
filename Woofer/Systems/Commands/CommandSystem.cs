using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using WooferGame.Controller.Commands;
using WooferGame.Systems.Interaction;

namespace WooferGame.Systems.Commands
{
    [ComponentSystem("CommandSystem", ProcessingCycles.None),
        Listening(typeof(ActivationEvent))]
    class CommandSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event e)
        {
            if(e is ActivationEvent ae && ae.Affected.Components.Get<ResolutionChangeComponent>() is ResolutionChangeComponent resolutionChange)
            {
                Owner.Controller.CommandFired(new ResolutionChangeCommand(resolutionChange.Resolution));
            }
        }
    }
}
