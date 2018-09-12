using EntityComponentSystem.Interfaces.Input.Mouse;
using EntityComponentSystem.Util;

using GameInterfaces.Input;
using GameInterfaces.Input.Keyboard;

namespace WooferGame.Input
{
    class KeyboardInputMap : IInputMap
    {
        public string Name => "Keyboard and Mouse";

        public Vector2D Movement
        {
            get
            {
                Vector2D movement = new Vector2D();
                if (keyboard[Key.W].IsPressed()) movement += Vector2D.UnitJ;
                if (keyboard[Key.A].IsPressed()) movement -= Vector2D.UnitI;
                if (keyboard[Key.S].IsPressed()) movement -= Vector2D.UnitJ;
                if (keyboard[Key.D].IsPressed()) movement += Vector2D.UnitI;
                return movement;
            }
        }

        public Vector2D Orientation {
            get
            {
                Vector2D orientation = mouse.Position - new Vector2D(1280, 720) / 2;
                orientation.Y *= -1;
                return orientation;
            }
        }

        public ButtonState Jump => keyboard[Key.Space];

        public ButtonState Pulse => keyboard[Key.LeftControl];

        public ButtonState Interact => keyboard[Key.E];

        IKeyboard keyboard;
        IMouse mouse;

        public KeyboardInputMap(IKeyboard keyboard, IMouse mouse)
        {
            this.keyboard = keyboard;
            this.mouse = mouse;
        }

        public bool IsBeingUsed => keyboard.IsBeingUsed || mouse.IsBeingUsed;

        public void SetVibration(float amount) { }
    }
}
