using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Scenes;

namespace WooferGame.Controller.Commands
{
    class SceneChangeCommand : Command
    {
        public Scene NewScene;

        public SceneChangeCommand(Scene scene) => NewScene = scene;
    }
}
