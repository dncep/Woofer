using EntityComponentSystem.Util;

using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;

namespace WooferGame.Input
{
    class GamePadInputMap : IInputMap
    {
        public string Name => "Game Pad";

        public Vector2D Movement => gamePad.Thumbsticks.Left;

        public Vector2D Orientation => gamePad.Thumbsticks.Left;

        public ButtonState Run => gamePad.Triggers.Right.TriggerToButtonState();

        public ButtonState Jump => gamePad.Buttons.A;

        public ButtonState Pulse => gamePad.Buttons.X;

        public ButtonState Interact => gamePad.Buttons.B;

        public ButtonState Pause => gamePad.Buttons.Start;

        private IGamePad gamePad;

        public GamePadInputMap(IGamePad gamePad) => this.gamePad = gamePad;

        public bool IsBeingUsed => gamePad.IsBeingUsed;

        public int ButtonIconOffset => 0;

        public ButtonState Debug => gamePad.Buttons.RightStick;
        public Vector2D DebugMovement => Orientation;

        public ButtonState Quicksave => gamePad.Buttons.LeftBumper;
        public ButtonState Quickload => gamePad.Buttons.RightBumper;

        public void SetVibration(float amount) => gamePad.SetVibration(amount, amount);
    }
}
