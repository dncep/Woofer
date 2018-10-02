﻿using System;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Scenes;
using GameInterfaces.Audio;
using GameInterfaces.Controller;
using WooferGame.Controller.Commands;
using WooferGame.Controller.Game;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor;
using WooferGame.Scenes;

namespace WooferGame.Controller
{
    public class WooferController : IGameController
    {
        public Scene ActiveScene { get; internal set; }
        public IRenderingUnit RenderingUnit { get; internal set; }
        public IInputUnit InputUnit { get; set; }
        public IAudioUnit AudioUnit { get; set; }

        public InputMapManager InputManager { get; private set; }
        public bool Paused { get; set; }
        public SaveGame CurrentSave { get; internal set; } = new SaveGame("scenes");

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

            ISoundEffect music = AudioUnit["bgm1"];
            music.Looping = true;
            music.Volume = 0.7f;
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

        private bool ResolutionChanged = false;

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
                        Scene oldScene = ActiveScene;
                        ActiveScene = changeScene.NewScene;
                        oldScene.Dispose();
                        LevelRenderingLayer.LevelScreenSize = LevelRenderingLayer.DefaultLevelScreenSize;
                        ResolutionChanged = true;
                        break;
                    }
                case ResolutionChangeCommand changeResolution:
                    {
                        LevelRenderingLayer.LevelScreenSize = changeResolution.NewResolution;
                        ResolutionChanged = true;
                        break;
                    }
            }
        }

        public void Draw<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer)
        {
            if(ResolutionChanged)
            {
                screenRenderer.UpdateLayers(RenderingUnit);
                ResolutionChanged = false;
            }
            RenderingUnit.Draw(screenRenderer);
        }
    }
}
