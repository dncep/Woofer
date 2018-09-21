﻿using EntityComponentSystem.Scenes;

using WooferGame.Systems.Player;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes
{
    class MainMenuScene : Scene
    {
        public MainMenuScene() : base(Woofer.Controller)
        {
            Entities.Add(new PlayerEntity(0, 0));

            Systems.Add(new LevelRenderer());
        }
    }
}
