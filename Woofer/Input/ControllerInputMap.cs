using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;
using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;

namespace WooferGame.Input
{
    class ControllerInputMap : IInputMap
    {
        public Vector2D Movement => gamePad.Thumbsticks.Left;

        public Vector2D Orientation => gamePad.Thumbsticks.Left;

        public ButtonState Jump => gamePad.Buttons.A;

        public ButtonState Pulse => gamePad.Buttons.X;

        private IGamePad gamePad;

        public ControllerInputMap(IGamePad gamePad) => this.gamePad = gamePad;
    }
}
