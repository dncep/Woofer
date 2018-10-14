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

namespace WooferGame.Systems.Enemies.Boss
{
    [ComponentSystem("BossSystem", ProcessingCycles.Update),
        Watching(typeof(PlayerComponent), typeof(Boss))]
    class BossSystem : ComponentSystem
    {
        public override void Update()
        {
            Entity player = WatchedComponents.OfType<PlayerComponent>().FirstOrDefault()?.Owner;
            foreach(Boss boss in WatchedComponents.OfType<Boss>())
            {
                Spatial sp = boss.Owner.GetComponent<Spatial>();
                Physical phys = boss.Owner.GetComponent<Physical>();

                Spatial playerSp = player?.GetComponent<Spatial>();
                if (sp == null || phys == null) continue;

                phys.GravityMultiplier = 1;
                phys.Velocity = new Vector2D(0, 362) * Owner.DeltaTime;
                /*boss.HoverTimer += Owner.DeltaTime;

                Vector2D hoverVelocity = new Vector2D(0, 16 * Math.Sin(Math.PI*boss.HoverTimer/2));

                phys.Velocity += hoverVelocity;*/

                Vector2D? flyTo = null;
                float flySpeed = 1;

                if(boss.State == Boss.Circling)
                {
                    if(boss.Transitioning)
                    {
                        if ((sp.Position - new Vector2D(0, 128)).Magnitude < 1)
                        {
                            boss.Transitioning = false;
                            boss.StateData = 0;
                        }
                        else
                        {
                            flyTo = new Vector2D(0, 128);
                            flySpeed = 2;
                        }
                    }
                    if(!boss.Transitioning)
                    {
                        boss.StateData += Owner.DeltaTime;
                        if(boss.StateData > 16)
                        {
                            boss.ChangeState(Boss.Laser);
                        } else
                        {
                            phys.Velocity += new Vector2D(125.6 * Math.Cos(Math.PI * boss.StateData / 4), 0);
                        }
                    }
                } else if(boss.State == Boss.Laser)
                {
                    if(boss.Transitioning)
                    {
                        if ((sp.Position - new Vector2D(0, 256)).Magnitude < 1)
                        {
                            boss.Transitioning = false;
                            boss.StateData = 0;
                        }
                        else
                        {
                            flyTo = new Vector2D(0, 256);
                            flySpeed = 2;
                        }
                    }
                    if(!boss.Transitioning)
                    {
                        boss.StateData += Owner.DeltaTime;
                        if (boss.StateData < 4)
                        {
                            if(playerSp != null)
                            {
                                flyTo = new Vector2D(playerSp.Position.X, sp.Position.Y);
                            }
                        } else
                        {
                            boss.ChangeState(Boss.Circling);
                        }
                    }
                }

                //Functions
                if(flyTo != null)
                {
                    phys.Velocity += flySpeed * (flyTo.Value - sp.Position);
                }
            }
        }
    }
}
