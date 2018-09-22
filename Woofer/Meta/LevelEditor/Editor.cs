using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using GameInterfaces.Controller;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor.Systems;
using WooferGame.Scenes;

namespace WooferGame.Meta.LevelEditor
{
    class Editor : IntroScene
    {

        internal static InputRepeatingTimeframe CycleTimeframe = new InputRepeatingTimeframe(15, 3);
        internal static InputTimeframe SelectTimeframe = new InputTimeframe(1);

        public Editor() : base()
        {
            Controller.Paused = true;

            Systems.Add(new EntityOutliner());
            Systems.Add(new EditorCursorSystem());
            Systems.Add(new EditorRendering());
            Systems.Add(new EntityListSystem());
            Systems.Add(new EntityViewSystem());

            Systems.Add(new ModalFocusSystem());
        }
    }
}
