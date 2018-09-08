using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Scenes;
using WooferGame.Systems.Player;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes
{
    class MainMenuScene : Scene
    {
        public MainMenuScene()
        {
            Entities.Add(new PlayerEntity(0, 0));

            Systems.Add(new LevelRenderer());
        }
    }
}
