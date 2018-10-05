using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Systems.Linking;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("player_orientation_system", ProcessingCycles.Input | ProcessingCycles.Render),
        Watching(typeof(PlayerOrientation))]
    class PlayerOrientationSystem : ComponentSystem
    {
        private readonly double deadzone = 0.1;

        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D thumbstick = inputMap.Orientation;

            if(thumbstick.Magnitude >= deadzone)
            {
                foreach(PlayerOrientation po in WatchedComponents)
                {
                    po.Unit = thumbstick.Normalize();
                    po.Owner.Components.Get<FollowedComponent>().Offset = po.OriginOffset + po.Unit * 24;
                }
            }
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            foreach(PlayerOrientation po in WatchedComponents)
            {
                Spatial sp = po.Owner.Components.Get<Spatial>();
                if (sp == null) continue;

                Vector2D origin = sp.Position;
                origin += po.OriginOffset;
                origin -= Owner.CurrentViewport.Location;
                System.Drawing.Size layerSize = r.GetLayerGraphics("level").GetSize();
                origin.X /= layerSize.Width;
                origin.Y /= layerSize.Height;
                System.Drawing.Size screenSize = Woofer.Controller.RenderingUnit.ScreenSize;
                origin.X *= screenSize.Width;
                origin.Y *= screenSize.Height;

                origin.X += screenSize.Width / 2;
                origin.Y += screenSize.Height / 2;

                origin.Y = (screenSize.Height) - origin.Y;
                break;
            }
        }
    }
}
