using EntityComponentSystem.Interfaces.Input.Mouse;
using EntityComponentSystem.Util;

using GameInterfaces.Input;
using GameInterfaces.Input.Keyboard;

namespace WooferGame.Input
{
    class KeyboardInputMap : IInputMap, IScreenAwareInput
    {
        public string Name => "Keyboard and Mouse";

        public Vector2D Origin;

        public Vector2D Movement
        {
            get
            {
                Vector2D movement = new Vector2D();
                if (keyboard[Key.Up].IsPressed()) movement += Vector2D.UnitJ;
                if (keyboard[Key.Left].IsPressed()) movement -= Vector2D.UnitI;
                if (keyboard[Key.Down].IsPressed()) movement -= Vector2D.UnitJ;
                if (keyboard[Key.Right].IsPressed()) movement += Vector2D.UnitI;
                return movement.Unit();
            }
        }

        public Vector2D Orientation => Movement;

        public ButtonState Jump => keyboard[Key.Z];

        public ButtonState Pulse => keyboard[Key.X];

        public ButtonState Interact => keyboard[Key.C];

        public ButtonState Debug => keyboard[Key.F3];
        public Vector2D DebugMovement => Movement;

        public ButtonState Quicksave => keyboard[Key.F5];
        public ButtonState Quickload => keyboard[Key.F7];

        IKeyboard keyboard;
        IMouse mouse;

        public KeyboardInputMap(IKeyboard keyboard, IMouse mouse)
        {
            this.keyboard = keyboard;
            this.mouse = mouse;
        }

        public bool IsBeingUsed => keyboard.IsBeingUsed || mouse.IsBeingUsed;

        public string IconSpritesheet => "keyboard_icons";

        public void SetVibration(float amount) { }

        public void SetOrientationOrigin(Vector2D origin) => Origin = origin;
    }
}
