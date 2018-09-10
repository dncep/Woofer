using System;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

using GameInterfaces.Controller;

using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Visual;
using WooferGame.Systems.Visual.Particles;

namespace WooferGame.Systems.Player.Animation
{
    [ComponentSystem("player_animation_system", ProcessingCycles.Render),
        Watching(typeof(PlayerAnimation)),
        Listening(typeof(PlayerJumpEvent), typeof(PulseEvent))]
    class PlayerAnimationSystem : ComponentSystem
    {
        private const int Legs = 0;
        private const int Torso = 1;
        private const int Head = 2;
        private const int Woofer = 3;
        private const int Arms = 4;

        private static readonly int[] Origins = { 0, 96, 128, 160, 352 };

        private static readonly Vector2D OrientationOffset = new Vector2D(256, 0);

        private static Rectangle Destination => new Rectangle(-16, 0, 32, 32);

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            foreach(PlayerAnimation player in WatchedComponents)
            {
                Renderable renderable = player.Owner.Components.Get<Renderable>();

                Vector2D[] srcOffsets = new Vector2D[5];
                srcOffsets[Legs] = new Vector2D(0, Origins[Legs]);
                srcOffsets[Torso] = new Vector2D(0, Origins[Torso]);
                srcOffsets[Head] = new Vector2D(0, Origins[Head]);
                srcOffsets[Woofer] = new Vector2D(0, Origins[Woofer]);
                srcOffsets[Arms] = new Vector2D(0, Origins[Arms]);

                Vector2D[] destOffsets = new Vector2D[5];
                destOffsets[Legs] = destOffsets[Torso] = destOffsets[Head] = destOffsets[Woofer] = destOffsets[Arms] = Vector2D.Empty;

                if(!player.Initialized)
                {
                    renderable.Sprites = new Sprite[]
                    {
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Legs], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Torso], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Head], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Woofer], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Arms], 32, 32))
                    };
                    player.Initialized = true;
                }

                Physical physical = player.Owner.Components.Get<Physical>();
                PlayerMovementComponent movement = player.Owner.Components.Get<PlayerMovementComponent>();
                PlayerOrientation orientation = player.Owner.Components.Get<PlayerOrientation>();
                PulseAbility pulse = player.Owner.Components.Get<PulseAbility>();

                if (orientation.Unit.X > 0 || player.LastLookedRight)
                {
                    for (int i = Legs; i <= Arms; i++) srcOffsets[i] += OrientationOffset;
                }

                if(orientation.Unit.Y >= Math.Sin(Math.PI/6))
                {

                    srcOffsets[Head].X += 32;
                    srcOffsets[Woofer].X += 32;
                    srcOffsets[Arms].X += 32;
                    if(orientation.Unit.Y >= Math.Sin(2*Math.PI/6))
                    {
                        srcOffsets[Woofer].X += 32;
                        srcOffsets[Arms].X += 32;
                    }
                } else if(orientation.Unit.Y <= Math.Sin(-Math.PI/6))
                {
                    srcOffsets[Head].X += 64;
                    srcOffsets[Woofer].X += 96;
                    srcOffsets[Arms].X += 96;
                    if (orientation.Unit.Y <= Math.Sin(-2 * Math.PI / 6))
                    {
                        srcOffsets[Woofer].X += 32;
                        destOffsets[Woofer] += new Vector2D(0, -3); //Offset woofer down since it goes out of the spritesheet grid
                        srcOffsets[Arms].X += 32;
                    }
                }

                if (orientation.Unit.X != 0) player.LastLookedRight = orientation.Unit.X > 0;

                if(pulse != null)
                {
                    srcOffsets[Woofer].Y += 32 * 5*(1-(pulse.EnergyMeter / pulse.MaxEnergy));
                }

                if (!movement.OnGround || Math.Abs(physical.Velocity.X) <= 1e-2) player.WalkAnimationProgress = 0;
                else if(Math.Abs(physical.Velocity.X) > 1e-2)
                {
                    if (orientation.Unit.X == 0 || orientation.Unit.X / Math.Abs(orientation.Unit.X) == physical.Velocity.X / Math.Abs(physical.Velocity.X)) player.WalkAnimationProgress++;
                    else player.WalkAnimationProgress--;
                    int frameDuration = 8;
                    int animationFrames = 6;
                    if (player.WalkAnimationProgress >= animationFrames * frameDuration) player.WalkAnimationProgress = 0;
                    else if (player.WalkAnimationProgress <= 0) player.WalkAnimationProgress = animationFrames * frameDuration - 1;
                    srcOffsets[Legs].X += 32 * (1 + (player.WalkAnimationProgress / frameDuration));

                    for (int i = Legs; i <= Arms; i++)
                    {
                        if(i != Legs && i != Head && player.WalkAnimationProgress/(animationFrames*2) % 2 == 0)
                        {
                            destOffsets[i] += new Vector2D(0, -1);
                        }
                        if(i == Head && (player.WalkAnimationProgress + frameDuration/2) / (animationFrames*2) % 2 != 0)
                        {
                            destOffsets[i] += new Vector2D(0, -1);
                        }
                    }
                }
                if(!movement.OnGround)
                {
                    srcOffsets[Legs] += new Vector2D(32, 32);
                }

                for(int i = Legs; i <= Arms; i++)
                {
                    renderable.Sprites[i].Source = new Rectangle(srcOffsets[i], 32, 32);
                    renderable.Sprites[i].Destination = Destination + destOffsets[i];
                }
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is PlayerJumpEvent je)
            {
                if(je.Sender.Owner.Components.Has<PlayerAnimation>() && je.Sender.Owner.Components.Get<Spatial>() is Spatial sp)
                {
                    Owner.Entities.Add(new CloudParticle(sp.Position + 2 * Vector2D.UnitJ + new Vector2D(-3, 0)));
                    Owner.Entities.Add(new CloudParticle(sp.Position + 2 * Vector2D.UnitJ + new Vector2D(3, 1)));
                }
            } else if(e is PulseEvent pe)
            {
                if(pe.Sender.Owner.Components.Has<PlayerAnimation>())
                {
                    Owner.Entities.Add(new CloudParticle(pe.Source + pe.Direction * 16));
                }
            }
        }
    }
}
