using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityComponentSystem.Scenes;
using WooferGame.Meta;

namespace WooferGame.Controller.Game
{
    public class SaveGame
    {
        public string DirectoryName;
        private Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        private Dictionary<string, Task<Scene>> LoadingTasks = new Dictionary<string, Task<Scene>>();

        public SaveGame(string directoryName) => DirectoryName = directoryName;

        public Scene GetScene(string sceneName)
        {
            return PrepareScene(sceneName).Result;
        }

        public async Task<Scene> PrepareScene(string sceneName)
        {
            if (Scenes.ContainsKey(sceneName)) return Scenes[sceneName];
            if(LoadingTasks.ContainsKey(sceneName))
            {
                return await LoadingTasks[sceneName];
            }
            WooferLoadOperation loader = new WooferLoadOperation(Woofer.Controller, sceneName, DirectoryName);
            Task<Scene> task = Task.Run<Scene>(() => {
                Scene scene = loader.Load();
                Scenes[sceneName] = scene;
                LoadingTasks.Remove(sceneName);
                Console.WriteLine($"Successfully loaded scene '{scene.Name}'");
                return scene;
            });

            LoadingTasks[sceneName] = task;
            return await task;
        }
    }
}
