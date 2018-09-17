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
using WooferGame.Scenes.LevelObjects;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("general_player_system", ProcessingCycles.None),
        Listening(typeof(SoftCollisionEvent))]
    class GeneralPlayerSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event evt)
        {
            if(evt is SoftCollisionEvent ce && ce.Victim.Components.Has<PlayerComponent>() && ce.Sender.Owner.Components.Has<WooferGiverComponent>())
            {
                ce.Sender.Owner.Active = false;
                if(!ce.Victim.Components.Has<PulseAbility>())
                {
                    ce.Victim.Components.Add(new PulseAbility() { Offset = new Vector2D(0, 16) });
                }
            }
        }
    }
}
