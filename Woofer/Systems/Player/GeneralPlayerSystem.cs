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
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Visual;

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

                    int spriteSourceX = Woofer.Controller.InputManager.ActiveInputMap.ButtonIconOffset;

                    Owner.Events.InvokeEvent(new ShowTextEvent(new Sprite("gui", new Rectangle(0, 0, 9, 9), new Rectangle(spriteSourceX, 9, 9, 9)), "Fire", ce.Sender) { Duration = 10 });
                    Owner.Events.InvokeEvent(new ShowTextEvent(new Sprite("gui", new Rectangle(0, 0, 9, 9), new Rectangle(spriteSourceX, 18, 9, 9)), "Aim", ce.Sender) { Duration = 10 });
                }
            }
        }
    }
}
