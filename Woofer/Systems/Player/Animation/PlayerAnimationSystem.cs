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
        private const int Head = 0;
        private const int Legs = 1;
        private const int Woofer = 2;
        private const int Arms = 3;

        private static readonly int[] Origins = { 0, 32, 128, 320 };

        private static readonly Vector2D OrientationOffset = new Vector2D(256, 0);

        private static Rectangle Destination => new Rectangle(-16, 0, 32, 32);

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            foreach(PlayerAnimation player in WatchedComponents)
            {
                Renderable renderable = player.Owner.Components.Get<Renderable>();

                Vector2D[] offsets = new Vector2D[4];
                offsets[Head] = new Vector2D(0, Origins[Head]);
                offsets[Legs] = new Vector2D(0, Origins[Legs]);
                offsets[Woofer] = new Vector2D(0, Origins[Woofer]);
                offsets[Arms] = new Vector2D(0, Origins[Arms]);


                if(!player.Initialized)
                {
                    renderable.Sprites = new Sprite[]
                    {
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(offsets[Head], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(offsets[Legs], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(offsets[Woofer], 32, 32)),
                        new Sprite(player.SpritesheetName, Destination, new Rectangle(offsets[Arms], 32, 32))
                    };
                    player.Initialized = true;

                }

                Physical physical = player.Owner.Components.Get<Physical>();
                PlayerOrientation orientation = player.Owner.Components.Get<PlayerOrientation>();
                PulseAbility pulse = player.Owner.Components.Get<PulseAbility>();

                if (orientation.Unit.X > 0 || player.LastLookedRight)
                {
                    for (int i = Head; i <= Arms; i++) offsets[i] += OrientationOffset;
                }
                if(orientation.Unit.Y >= Math.Sin(Math.PI/6))
                {
                    offsets[Head].X += 32;
                } else if(orientation.Unit.Y <= Math.Sin(-Math.PI/6))
                {
                    offsets[Head].X += 64;
                }

                if (orientation.Unit.X != 0) player.LastLookedRight = orientation.Unit.X > 0;

                if(pulse != null)
                {
                    offsets[Woofer].Y += 32 * 5*(1-(pulse.EnergyMeter / pulse.MaxEnergy));
                }

                for(int i = Head; i <= Arms; i++)
                {
                    renderable.Sprites[i].Source = new Rectangle(offsets[i], 32, 32);
                }
            }
        }
    }
}
