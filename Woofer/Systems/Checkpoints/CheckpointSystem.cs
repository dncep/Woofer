using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Checkpoints
{
    [ComponentSystem("checkpoint_system")]
    class CheckpointSystem : ComponentSystem
    {

        public CheckpointSystem()
        {
            Watching = new string[] { Component.IdentifierOf<CheckpointComponent>() };
            Listening = new string[] { Event.IdentifierOf<CheckpointRequestEvent>() };
        }

        public override void EventFired(object sender, Event re)
        {
            if(re is CheckpointRequestEvent e)
            {
                Spatial sp = e.EntityToMove.Components.Get<Spatial>();
                Physical ph = e.EntityToMove.Components.Get<Physical>();

                foreach(CheckpointComponent checkpoint in WatchedComponents)
                {
                    if(checkpoint.Selected)
                    {
                        if(sp != null) sp.Position = checkpoint.Owner.Components.Get<Spatial>().Position;
                        if(ph != null) ph.Velocity = Vector2D.Empty;
                        break;
                    }
                }
            }
        }
    }
}
