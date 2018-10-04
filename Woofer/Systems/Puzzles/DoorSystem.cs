using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using WooferGame.Systems.Interaction;

namespace WooferGame.Systems.Puzzles
{
    [ComponentSystem("door_system", ProcessingCycles.Update),
        Watching(typeof(DoorComponent)),
        Listening(typeof(ActivationEvent))]
    class DoorSystem : ComponentSystem
    {
        public override void Update()
        {
            foreach(DoorComponent door in WatchedComponents)
            {
                if(door.OpeningDirection != 0)
                {
                    double delta = door.OpeningDirection * (door.MaxOpenDistance / door.OpeningTime) * Owner.DeltaTime;
                    if(delta > 0 && door.CurrentOpenDistance + delta > door.MaxOpenDistance)
                    {
                        delta = door.MaxOpenDistance - door.CurrentOpenDistance;
                    } else if(delta < 0 && door.CurrentOpenDistance + delta < 0)
                    {
                        delta = -door.CurrentOpenDistance;
                    }

                    door.OpeningDirection = delta;

                    if(delta != 0)
                    {
                        Spatial sp = door.Owner.Components.Get<Spatial>();
                        sp.Position += new Vector2D(0, delta);
                        door.CurrentOpenDistance += delta;
                    }
                }
            }
        }
        public override void EventFired(object sender, Event evt)
        {
            if(evt is ActivationEvent ae && ae.Affected.Components.Get<DoorComponent>() is DoorComponent door)
            {
                int multiplier = 1;
                if(door.Toggle)
                {
                    if(door.OpeningDirection == 0)
                    {
                        multiplier = (door.CurrentOpenDistance < door.MaxOpenDistance) ? 1 : -1;
                    } else
                    {
                        multiplier = Math.Sign(door.OpeningDirection);
                    }
                }
                door.OpeningDirection = multiplier;
            }
        }
    }
}
