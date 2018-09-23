using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using GameInterfaces.Input;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor.Systems;
using WooferGame.Meta.LevelEditor.Systems.ComponentView;
using WooferGame.Meta.LevelEditor.Systems.CursorModes;
using WooferGame.Meta.LevelEditor.Systems.InputModes;
using WooferGame.Scenes;

namespace WooferGame.Meta.LevelEditor
{
    class Editor : IntroScene
    {
        internal static InputRepeatingTimeframe MoveTimeframe = new InputRepeatingTimeframe(15, 3);
        internal static InputTimeframe SelectTimeframe = new InputTimeframe(1);
        internal static InputRepeatingTimeframe SelectRepeatingTimeframe = new InputRepeatingTimeframe(15, 3);

        public Editor() : base()
        {
            Controller.Paused = true;

            Systems.Add(new EntityOutliner());
            Systems.Add(new EditorCursorSystem());
            Systems.Add(new EditorRendering());
            Systems.Add(new EntityListSystem());
            Systems.Add(new EntityViewSystem());

            Systems.Add(new MoveCursorModeSystem());

            Systems.Add(new NumberInputSystem());
            Systems.Add(new ComponentSelectViewSystem());

            Systems.Add(new ModalFocusSystem());
        }

        public override void InvokeInput()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;
            
            Editor.MoveTimeframe.RegisterState(inputMap.Movement.Magnitude > 1e-5 ? ButtonState.Pressed : ButtonState.Released);
            Editor.SelectTimeframe.RegisterState(inputMap.Jump);
            Editor.SelectRepeatingTimeframe.RegisterState(inputMap.Jump);

            base.InvokeInput();
        }
    }
}
