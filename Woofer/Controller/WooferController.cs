using System;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Scenes;
using GameInterfaces.Audio;
using GameInterfaces.Controller;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Scenes;

namespace WooferGame.Controller
{
    class WooferController : IGameController
    {
        public Scene ActiveScene { get; internal set; }
        public IRenderingUnit RenderingUnit { get; internal set; }
        public IInputUnit InputUnit { get; set; }
        public IAudioUnit AudioUnit { get; set; }

        public InputMapManager InputManager { get; private set; }

        public void Initialize() {
            InputManager = new InputMapManager(this);
            InputManager.Add(new KeyboardInputMap(InputUnit.Keyboard, InputUnit.Mouse));
            InputManager.Add(new GamePadDualInputMap(InputUnit.GamePads[0]));

            AudioUnit.Load("pulse_low_alt");
            AudioUnit.Load("pulse_low");
            AudioUnit.Load("pulse_mid");
            AudioUnit.Load("pulse_high");
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

        public void CommandFired(Command command)
        {
            switch(command)
            {
                case OrientationOriginChangeCommand changeOrigin:
                    {
                        if (InputManager.ActiveInputMap is IScreenAwareInput screenAware)
                        {
                            screenAware.SetOrientationOrigin(changeOrigin.NewOrigin);
                        }
                        break;
                    }
                case SceneChangeCommand changeScene:
                    {
                        ActiveScene.Dispose();
                        ActiveScene = changeScene.NewScene;
                        break;
                    }
            }
        }
    }
}
