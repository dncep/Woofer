using EntityComponentSystem.Util;

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
        ButtonState Interact { get; }

        bool IsBeingUsed { get; }
        int ButtonIconOffset { get; }
        
        ButtonState Debug { get; }
        Vector2D DebugMovement { get; }
        ButtonState Quicksave { get; }
        ButtonState Quickload { get; }

        void SetVibration(float amount);
    }
}
