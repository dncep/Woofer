using EntityComponentSystem.Util;

using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;

namespace WooferGame.Input
{
    class GamePadDualInputMap : IInputMap
    {
        public string Name => "Game Pad (Dual)";

        public Vector2D Movement => gamePad.Thumbsticks.Left;

        public Vector2D Orientation => gamePad.Thumbsticks.Right;

        public ButtonState Jump => gamePad.Buttons.A;

        public ButtonState Pulse => gamePad.Triggers.Right.TriggerToButtonState();

        public ButtonState Interact => gamePad.Buttons.B;

        private IGamePad gamePad;

        public GamePadDualInputMap(IGamePad gamePad) => this.gamePad = gamePad;
        
        public bool IsBeingUsed => gamePad.IsBeingUsed;

        public int ButtonIconOffset => 0;

        public ButtonState Debug => gamePad.Buttons.RightStick;
        public Vector2D DebugMovement => Orientation;

        public ButtonState Quicksave => gamePad.Buttons.LeftBumper;
        public ButtonState Quickload => gamePad.Buttons.RightBumper;

        public void SetVibration(float amount) => gamePad.SetVibration(amount, amount);
    }

    class GamePadClassicInputMap : IInputMap
    {
        public string Name => "Game Pad (Classic)";

        public Vector2D Movement => gamePad.Thumbsticks.Left;

        public Vector2D Orientation => gamePad.Thumbsticks.Left;

        public ButtonState Jump => gamePad.Buttons.A;

        public ButtonState Pulse => gamePad.Buttons.B;

        public ButtonState Interact => gamePad.Buttons.B;

        private IGamePad gamePad;

        public GamePadClassicInputMap(IGamePad gamePad) => this.gamePad = gamePad;

        public bool IsBeingUsed => gamePad.IsBeingUsed;

        public int ButtonIconOffset => 0;

        public ButtonState Debug => gamePad.Buttons.RightStick;
        public Vector2D DebugMovement => Orientation;

        public ButtonState Quicksave => gamePad.Buttons.LeftBumper;
        public ButtonState Quickload => gamePad.Buttons.RightBumper;

        public void SetVibration(float amount) => gamePad.SetVibration(amount, amount);
    }
}
