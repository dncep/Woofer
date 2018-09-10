﻿using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;

using WooferGame.Input;
using WooferGame.Systems.Linking;

namespace WooferGame.Systems.Player
{
    [ComponentSystem("player_orientation_system", ProcessingCycles.Input | ProcessingCycles.Tick),
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
                    po.Unit = thumbstick.Unit();
                    po.Owner.Components.Get<FollowedComponent>().Offset = new Vector2D(0, 16) + po.Unit * 24;
                }
            }
        }

        public override void Tick() => base.Tick();
    }
}
