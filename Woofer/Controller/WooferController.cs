using System;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Scenes;
using GameInterfaces.Audio;
using GameInterfaces.Controller;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor;
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
        public bool Paused { get; set; }

        public WooferController()
        {
            RenderingUnit = new WooferRenderingUnit(this);
        }

        public void Initialize() {
            InputManager = new InputMapManager(this);
            InputManager.Add(new GamePadInputMap(InputUnit.GamePads[0]));
            InputManager.Add(new KeyboardInputMap(InputUnit.Keyboard, InputUnit.Mouse));

            AudioUnit.Load("pulse_low_alt");
            AudioUnit.Load("pulse_low");
            AudioUnit.Load("pulse_mid");
            AudioUnit.Load("pulse_high");

            AudioUnit.Load("select");

            AudioUnit.Load("bgm");
            AudioUnit.Load("bgm1");

            ISoundEffect music = AudioUnit["bgm"];
            music.Looping = true;
            music.Volume = 0.05f;
            music.Play();

            ActiveScene = new MainMenuScene();
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
