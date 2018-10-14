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

namespace WooferGame.Systems.Enemies.Boss
{
    [ComponentSystem("BossSystem", ProcessingCycles.Update),
        Watching(typeof(Boss))]
    class BossSystem : ComponentSystem
    {
        public override void Update()
        {
            foreach(Boss boss in WatchedComponents)
            {
                Spatial sp = boss.Owner.GetComponent<Spatial>();
                Physical phys = boss.Owner.GetComponent<Physical>();
                if (sp == null || phys == null) continue;

                phys.GravityMultiplier = 1;
                phys.Velocity = new Vector2D(0, 362) * Owner.DeltaTime;
                boss.HoverTimer += Owner.DeltaTime;

                Vector2D hoverVelocity = new Vector2D(0, 16 * Math.Sin(Math.PI*boss.HoverTimer/2));

                phys.Velocity += hoverVelocity;
            }
        }
    }
}
