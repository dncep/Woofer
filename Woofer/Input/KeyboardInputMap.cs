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
                if (keyboard[Key.W].IsPressed()) movement += Vector2D.UnitJ;
                if (keyboard[Key.A].IsPressed()) movement -= Vector2D.UnitI;
                if (keyboard[Key.S].IsPressed()) movement -= Vector2D.UnitJ;
                if (keyboard[Key.D].IsPressed()) movement += Vector2D.UnitI;
                return movement.Unit();
            }
        }

        public Vector2D Orientation {
            get
            {
                Vector2D orientation = mouse.Position - Origin;
                orientation.Y *= -1;
                return orientation;
            }
        }

        public ButtonState Jump => keyboard[Key.Space];

        public ButtonState Pulse => ButtonStateExtensions.ButtonOr(keyboard[Key.LeftControl], mouse.LeftButton);

        public ButtonState Interact => keyboard[Key.E];

        public ButtonState Debug => keyboard[Key.F3];
        public Vector2D DebugMovement => Movement;

        IKeyboard keyboard;
        IMouse mouse;

        public KeyboardInputMap(IKeyboard keyboard, IMouse mouse)
        {
            this.keyboard = keyboard;
            this.mouse = mouse;
        }

        public bool IsBeingUsed => keyboard.IsBeingUsed || mouse.IsBeingUsed;

        public int ButtonIconOffset => 9;

        public void SetVibration(float amount) { }

        public void SetOrientationOrigin(Vector2D origin) => Origin = origin;
    }
}
