using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using WooferGame.Systems.Checkpoints;

namespace WooferGame.Systems.DeathBarrier
{
    [ComponentSystem("death_barrier")]
    class DeathBarrierSystem : ComponentSystem
    {
        public DeathBarrierSystem()
        {
            Watching = new string[] {
                Component.IdentifierOf<DeathBarrierComponent>(),
                Component.IdentifierOf<RemoveOnBarrierComponent>(),
                Component.IdentifierOf<CheckpointOnBarrierComponent>() };

            TickProcessing = true;
        }

        private List<Type> typeOrder = new List<Type>()
        {
            typeof(DeathBarrierComponent),
            typeof(RemoveOnBarrierComponent),
            typeof(CheckpointOnBarrierComponent)
        };

        public override void Tick()
        {
            DeathBarrierComponent barrier = null;

            foreach(Component component in WatchedComponents.OrderBy(a => typeOrder.IndexOf(a.GetType())))
            {
                if(component is DeathBarrierComponent)
                {
                    barrier = component as DeathBarrierComponent;
                } else if(component is RemoveOnBarrierComponent)
                {
                    if(component.Owner.Components.Get<Spatial>() is Spatial sp)
                    {
                        if (sp.Y < (barrier?.Y ?? 0))
                        {
                            component.Owner.Active = false;
                        }
                    }
                } else if(component is CheckpointOnBarrierComponent)
                {
                    if(component.Owner.Components.Get<Spatial>() is Spatial sp)
                    {
                        if(sp.Y < (barrier?.Y ?? 0))
                        {
                            Console.WriteLine("Crossing barrier; component is " + barrier);
                            Owner.Events.InvokeEvent(new CheckpointRequestEvent(barrier, component.Owner));
                        }
                    }
                }
            }
        }
    }
}
