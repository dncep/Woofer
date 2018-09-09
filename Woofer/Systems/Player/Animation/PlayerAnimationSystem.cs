using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Player.Animation
{
    [ComponentSystem("player_animation_system", ProcessingCycles.Render),
        Watching(typeof(PlayerAnimation))]
    class PlayerAnimationSystem : ComponentSystem
    {
        private const int Torso = 0;
        private const int Head = 1;
        private const int Legs = 2;
        private const int Woofer = 3;
        private const int Arms = 4;

        private static readonly int[] Origins = { 0, 32, 64, 160, 352 };

        private static readonly Vector2D OrientationOffset = new Vector2D(256, 0);

        private static Rectangle Destination => new Rectangle(-16, 0, 32, 32);

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            foreach(PlayerAnimation player in WatchedComponents)
            {
                Renderable renderable = player.Owner.Components.Get<Renderable>();

                Vector2D[] srcOffsets = new Vector2D[5];
                srcOffsets[Torso] = new Vector2D(0, Origins[Torso]);
                srcOffsets[Head] = new Vector2D(0, Origins[Head]);
                srcOffsets[Legs] = new Vector2D(0, Origins[Legs]);
                srcOffsets[Woofer] = new Vector2D(0, Origins[Woofer]);
                srcOffsets[Arms] = new Vector2D(0, Origins[Arms]);

                Vector2D[] destOffsets = new Vector2D[5];
                destOffsets[Torso] = destOffsets[Head] = destOffsets[Legs] = destOffsets[Woofer] = destOffsets[Arms] = Vector2D.Empty;
                destOffsets[Head] += new Vector2D(0, 0);

                if(!player.Initialized)
                {
                    renderable.Sprites = new Sprite[]
                    {
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Torso], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Head], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Legs], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Woofer], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(srcOffsets[Arms], 32, 32))
                    };
                    player.Initialized = true;
                }

                Physical physical = player.Owner.Components.Get<Physical>();
                PlayerOrientation orientation = player.Owner.Components.Get<PlayerOrientation>();
                PulseAbility pulse = player.Owner.Components.Get<PulseAbility>();

                if (orientation.Unit.X > 0 || player.LastLookedRight)
                {
                    for (int i = Torso; i <= Arms; i++) srcOffsets[i] += OrientationOffset;
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

                for(int i = Torso; i <= Arms; i++)
                {
                    renderable.Sprites[i].Source = new Rectangle(srcOffsets[i], 32, 32);
                    renderable.Sprites[i].Destination = Destination + destOffsets[i];
                }
            }
        }
    }
}
