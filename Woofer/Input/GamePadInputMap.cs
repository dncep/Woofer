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

        public ButtonState Jump => gamePad.Buttons.A;

        public ButtonState Pulse => gamePad.Buttons.X;


        private IGamePad gamePad;

        public GamePadInputMap(IGamePad gamePad) => this.gamePad = gamePad;
        
        public bool IsBeingUsed => gamePad.IsBeingUsed;

    }
}
