﻿using EntityComponentSystem.Util;

using GameInterfaces.Input;

namespace WooferGame.Input
{
    interface IInputMap
    {
        string Name { get; }

        Vector2D Movement { get; }
        Vector2D Orientation { get; }

        ButtonState Run { get; }

        ButtonState Jump { get; }
        ButtonState Pulse { get; }
        ButtonState Interact { get; }

        ButtonState Start { get; }

        bool IsBeingUsed { get; }
        string IconSpritesheet { get; }
        
        ButtonState Debug { get; }
        Vector2D DebugMovement { get; }
        ButtonState Quicksave { get; }
        ButtonState Quickload { get; }

        ButtonState Pause { get; }
        ButtonState Back { get; }

        void SetVibration(float amount);
    }
}
