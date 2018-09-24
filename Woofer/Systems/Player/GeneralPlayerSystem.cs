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
using WooferGame.Systems.HUD;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("general_player_system", ProcessingCycles.None),
        Watching(typeof(PlayerComponent)),
        Listening(typeof(ActivationEvent))]
    class GeneralPlayerSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event evt)
        {
            if(evt is ActivationEvent ae && ae.Affected.Components.Has<WooferGiverComponent>())
            {
                Entity player = WatchedComponents.FirstOrDefault()?.Owner;
                if (player == null) return;
                ae.Affected.Active = false;
                if(!player.Components.Has<PulseAbility>())
                {
                    player.Components.Add(new PulseAbility());
                    
                    Owner.Events.InvokeEvent(new ShowTextEvent(new Sprite("x_icons", new Rectangle(0, 0, 9, 9), new Rectangle(0, 9, 9, 9)) { Modifiers = Sprite.Mod_InputType }, "Activate", ae.Sender) { Duration = 10 });
                }
            }

        }
    }
}
