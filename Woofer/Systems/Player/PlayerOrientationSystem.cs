using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;

using WooferGame.Input;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("player_orientation_system", ProcessingCycles.Input | ProcessingCycles.Tick),
        Watching(typeof(PlayerOrientation))]
    class PlayerOrientationSystem : ComponentSystem
    {
        private readonly double deadzone = 0.3;

        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            Vector2D thumbstick = inputMap.Orientation;

            if(thumbstick.Magnitude >= deadzone)
            {
                foreach(PlayerOrientation po in WatchedComponents)
                {
                    po.Unit = thumbstick.Unit();
                }
            }
        }

        public override void Tick() => base.Tick();
    }
}
