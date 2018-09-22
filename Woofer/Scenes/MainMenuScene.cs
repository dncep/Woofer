using EntityComponentSystem.Scenes;
using WooferGame.Scenes.Menu;
using WooferGame.Systems.Player;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes
{
    class MainMenuScene : Scene
    {
        public MainMenuScene()
        {
            Systems.Add(new MenuSystem());

        }
    }
}
