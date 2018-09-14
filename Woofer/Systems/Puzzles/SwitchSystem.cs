﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Puzzles
{
    //Should be added **BEFORE** the physics system
    [ComponentSystem("switches", ProcessingCycles.Tick),
        Listening(typeof(SoftCollisionEvent)),
        Watching(typeof(SwitchComponent))]
    class SwitchSystem : ComponentSystem
    {
        public override void Tick()
        {
            foreach(SwitchComponent switchComponent in WatchedComponents)
            {
                if(switchComponent.PressedState > 0) switchComponent.PressedState--;

            }
        }
        public override void EventFired(object sender, Event evt)
        {
            if(evt is SoftCollisionEvent ce && ce.Victim.Components.Has<SwitchComponent>())
            {
                SwitchComponent switchComponent = ce.Victim.Components.Get<SwitchComponent>();
                if(!switchComponent.Pressed)
                {
                    Owner.Events.InvokeEvent(new ActivationEvent(ce.Sender, switchComponent.Owner, ce));
                }
                switchComponent.PressedState = 2;
            }
        }
    }
}