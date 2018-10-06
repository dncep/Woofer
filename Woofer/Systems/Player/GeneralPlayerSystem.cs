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
using WooferGame.Controller.Commands;
using WooferGame.Scenes.LevelObjects;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.HealthSystems;
using WooferGame.Systems.HUD;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("general_player_system", ProcessingCycles.Update),
        Watching(typeof(PlayerComponent)),
        Listening(typeof(ActivationEvent), typeof(DeathEvent))]
    class GeneralPlayerSystem : ComponentSystem
    {
        public override void Update()
        {
            foreach(PlayerComponent player in WatchedComponents)
            {
                if(!player.Initialized)
                {
                    Entity entity = player.Owner;
                    if(Woofer.Controller.CurrentSave.Data.HasWoofer)
                    {
                        entity.Components.Add(new PulseAbility());
                        entity.Components.Get<PulseAbility>().MaxEnergy = Woofer.Controller.CurrentSave.Data.MaxEnergy;
                    } else
                    {
                        entity.Components.Remove<PulseAbility>();
                    }
                    if(!entity.Components.Has<Health>())
                    {
                        entity.Components.Add(new Health());
                    }
                    Health health = entity.Components.Get<Health>();
                    bool heal = health.CurrentHealth == health.MaxHealth;
                    health.MaxHealth = Woofer.Controller.CurrentSave.Data.MaxHealth;
                    if (heal) health.CurrentHealth = health.MaxHealth;
                    health.HealthBarOffset = new Vector2D(0, 32);
                    health.RemoveOnDeath = false;
                    player.Initialized = true;
                }
            }
        }

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
                    Woofer.Controller.CurrentSave.Data.HasWoofer = true;
                    Woofer.Controller.CommandFired(new SaveCommand());

                    Owner.Events.InvokeEvent(new ShowTextEvent(new Sprite("x_icons", new Rectangle(0, 0, 9, 9), new Rectangle(0, 9, 9, 9)) { Modifiers = Sprite.Mod_InputType }, "Activate", ae.Sender) { Duration = 10 });
                }
            } else if(evt is DeathEvent de && de.Affected.HasComponent<PlayerComponent>())
            {
                Health health = de.Affected.GetComponent<Health>();
                if(health != null)
                {
                    health.RemoveOnDeath = false;
                    health.CurrentHealth = health.MaxHealth;
                }
                Owner.Events.InvokeEvent(new CheckpointRequestEvent(de.Affected.GetComponent<PlayerComponent>(), de.Affected));
            }
        }
    }
}
