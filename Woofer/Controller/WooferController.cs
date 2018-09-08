using System;

using EntityComponentSystem.Scenes;

using GameInterfaces.Controller;

using WooferGame.Input;
using WooferGame.Scenes;

namespace WooferGame.Controller
{
    class WooferController : IGameController
    {
        public Scene ActiveScene { get; internal set; }
        public IRenderingUnit RenderingUnit { get; internal set; }
        public IInputUnit InputUnit { get; set; }

        public InputMapManager InputManager { get; private set; }

        public void Initialize() {
            InputManager = new InputMapManager();
            InputManager.Add(new KeyboardInputMap(InputUnit.Keyboard, InputUnit.Mouse));
            InputManager.Add(new GamePadInputMap(InputUnit.GamePads[0]));
        }


        public WooferController()
        {
            RenderingUnit = new WooferRenderingUnit(this);
            ActiveScene = new IntroScene();
        }


        public void Tick(TimeSpan timeSpan, TimeSpan elapsedGameTime)
        {
            ActiveScene.InvokeTick(timeSpan, elapsedGameTime);
        }
        public void Input()
        {
            InputManager.Tick();
            ActiveScene.InvokeInput();
        }

    }
}
