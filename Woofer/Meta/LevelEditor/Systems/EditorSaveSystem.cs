using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using WooferGame.Input;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("editor_save", ProcessingCycles.Input, ProcessingFlags.Pause)]
    class EditorSaveSystem : ComponentSystem
    {
        private bool Active = true;

        public string SceneName = null;
        public string SaveName = null;

        public override bool ShouldSave => false;

        public static readonly InputTimeframe SaveTimeframe = new InputTimeframe(1);

        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            SaveTimeframe.RegisterState(inputMap.Quicksave);

            if(SceneName != null && SaveName != null && SaveTimeframe.Execute())
            {

                
            }
        }
    }
}
