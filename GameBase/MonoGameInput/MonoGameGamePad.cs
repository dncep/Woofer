using EntityComponentSystem.Util;
using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;

namespace GameBase.MonoGameInput
{
    public class MonoGameGamePad : IGamePad
    {
        public IGamePadButtons Buttons { get; private set; }

        public IGamePadTriggers Triggers { get; private set; }

        public IGamePadThumbsticks Thumbsticks { get; private set; }

        public IGamePadDPad DPad { get; private set; }

        private int playerId;

        public MonoGameGamePad(int playerId)
        {
            this.playerId = playerId;

            Buttons = new MonoGameGamePadButtons(playerId);
            Triggers = new MonoGameGamePadTriggers(playerId);
            Thumbsticks = new MonoGameGamePadThumbsticks(playerId);
            DPad = new MonoGameGamePadDPad(playerId);
        }

        public void SetVibration(float left, float right) => GamePad.SetVibration(playerId, left, right);
    }

    class MonoGameGamePadButtons : IGamePadButtons
    {
        private Microsoft.Xna.Framework.Input.GamePadButtons Buttons => GamePad.GetState(playerId).Buttons;

        public ButtonState A =>             Buttons.A.ToInterface();
        public ButtonState B =>             Buttons.B.ToInterface();
        public ButtonState X =>             Buttons.X.ToInterface();
        public ButtonState Y =>             Buttons.Y.ToInterface();
        public ButtonState Start =>         Buttons.Start.ToInterface();
        public ButtonState Back =>          Buttons.Back.ToInterface();
        public ButtonState Home =>          Buttons.BigButton.ToInterface();
        public ButtonState LeftBumper =>    Buttons.LeftShoulder.ToInterface();
        public ButtonState RightBumper =>   Buttons.RightShoulder.ToInterface();
        public ButtonState LeftStick =>     Buttons.LeftStick.ToInterface();
        public ButtonState RightStick =>    Buttons.RightStick.ToInterface();

        private int playerId;
        public MonoGameGamePadButtons(int playerId)
        {
            this.playerId = playerId;
        }
    }

    class MonoGameGamePadTriggers : IGamePadTriggers
    {
        private Microsoft.Xna.Framework.Input.GamePadTriggers Triggers => GamePad.GetState(playerId).Triggers;

        public float Left =>    Triggers.Left;
        public float Right =>   Triggers.Right;

        private int playerId;
        public MonoGameGamePadTriggers(int playerId)
        {
            this.playerId = playerId;
        }
    }

    class MonoGameGamePadDPad : IGamePadDPad
    {
        private Microsoft.Xna.Framework.Input.GamePadDPad DPad => GamePad.GetState(playerId).DPad;

        public ButtonState Up =>    DPad.Up.ToInterface();
        public ButtonState Down =>  DPad.Down.ToInterface();
        public ButtonState Left =>  DPad.Left.ToInterface();
        public ButtonState Right => DPad.Right.ToInterface();

        private int playerId;
        public MonoGameGamePadDPad(int playerId)
        {
            this.playerId = playerId;
        }
    }

    class MonoGameGamePadThumbsticks : IGamePadThumbsticks
    {
        private Microsoft.Xna.Framework.Input.GamePadThumbSticks Thumbsticks => GamePad.GetState(playerId).ThumbSticks;

        public Vector2D Left =>     Thumbsticks.Left.ToInterface();
        public Vector2D Right =>    Thumbsticks.Right.ToInterface();

        private int playerId;
        public MonoGameGamePadThumbsticks(int playerId)
        {
            this.playerId = playerId;
        }
    }
}
