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
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;

namespace WooferGame.Systems.Enemies
{
    [ComponentSystem("ai_system", ProcessingCycles.Tick),
        Watching(typeof(PlayerComponent), typeof(SentryAI)),
        Listening(typeof(RigidCollisionEvent))]
    class AISystem : ComponentSystem
    {

        private static readonly Random Random = new Random();

        public override void Tick()
        {
            Entity player = WatchedComponents.FirstOrDefault()?.Owner;
            if (player == null) return;
            Spatial playerSp = player.Components.Get<Spatial>();
            if (playerSp == null) return;

            foreach(SentryAI sentry in WatchedComponents.Where(c => c is SentryAI))
            {
                if (sentry.ThrowTime > 0) sentry.ThrowTime--;
                Spatial sp = sentry.Owner.Components.Get<Spatial>();
                Physical phys = sentry.Owner.Components.Get<Physical>();
                if (sp == null || phys == null) continue;
                bool inRange = (sp.Position - playerSp.Position).Magnitude < sentry.FollowDistance;

                if (sentry.ThrowTime == 5)
                {
                    Vector2D velocity = new Vector2D();
                    double targetTime = 0.8;
                    velocity.X = (playerSp.Position.X - sp.Position.X) / targetTime;
                    velocity.Y = ((playerSp.Position.Y - sp.Position.Y) + (362 * targetTime * targetTime) / 2) / targetTime;
                    velocity = velocity.Normalize() * Math.Min(velocity.Magnitude, 256);

                    Owner.Entities.Add(new Projectile(sp.Position, velocity));
                }

                if (!sentry.OnGround) continue;

                if(inRange && sentry.ThrowTime == 0)
                {
                    sentry.ThrowTime = 80;
                }
            }
        }


        public override void EventFired(object sender, Event e)
        {
            if (e is RigidCollisionEvent ce)
            {
                if (ce.Victim.Components.Get<SentryAI>() is SentryAI ai && ce.Normal.Y > 0)
                {
                    ai.OnGround = true;
                }
            }
        }
    }
}
