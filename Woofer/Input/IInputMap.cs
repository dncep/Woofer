﻿using EntityComponentSystem.Util;

using GameInterfaces.Input;

namespace WooferGame.Input
{
    interface IInputMap
    {
        string Name { get; }

        Vector2D Movement { get; }
        Vector2D Orientation { get; }
        ButtonState Jump { get; }
        ButtonState Pulse { get; }

        bool IsBeingUsed { get; }

        void SetVibration(float amount);
    }
}
