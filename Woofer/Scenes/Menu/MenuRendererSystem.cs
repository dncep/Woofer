using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using GameInterfaces.Input;
using WooferGame.Common;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor;
using WooferGame.Meta.LevelSelect;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes.Menu
{
    [ComponentSystem("menu", ProcessingCycles.Input | ProcessingCycles.Render)]
    class MenuSystem : ComponentSystem
    {
        private static InputTimeframe StartInput = new InputTimeframe(1);
        private static InputTimeframe SelectInput = new InputTimeframe(1);

        private string[] OptionLabels = new[] { "Start", "Options", "Level Editor", "Quit" };
        private int SelectedIndex = 0;

        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            StartInput.RegisterState(inputMap.Jump);
            

            SelectInput.RegisterState(inputMap.Movement.Magnitude > 0 ? ButtonState.Pressed : ButtonState.Released);
            if(inputMap.Movement.Magnitude > 0)
            {
                if(inputMap.Movement.Y > 0 && SelectInput.Execute())
                {
                    SelectedIndex--;
                } else if(inputMap.Movement.Y < 0 && SelectInput.Execute())
                {
                    SelectedIndex++;
                }

                if (SelectedIndex > 3 ) SelectedIndex = 0;
                if (SelectedIndex < 0 ) SelectedIndex = 3;
            }


            if (inputMap.Jump.IsPressed() && StartInput.Execute())
            {
                switch (SelectedIndex)
                {
                    case 0:
                        Woofer.Controller.CommandFired(new SceneChangeCommand(new IntroScene()));
                        break;
                    case 1:
                        break;
                    case 2:
                        Woofer.Controller.CommandFired(new SceneChangeCommand(new LevelSelect()));
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;

                }
            }
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var layer = r.GetLayerGraphics("hud");

            layer.Draw(r.SpriteManager["MainSceneBG"], new System.Drawing.Rectangle(0, 0, 330, 180));

            int destY = 100;
            int index = 0;
            foreach(string label in OptionLabels)
            {
                TextUnit text;
                if (index == SelectedIndex) text = new TextUnit(new Sprite("gui", new EntityComponentSystem.Util.Rectangle(0, 0, 8, 8), new EntityComponentSystem.Util.Rectangle(0, 0, 8, 8)), label ,Color.Coral);
                else text = new TextUnit(label , Color.White);
                text.Render(r, layer, new System.Drawing.Point(8, destY));
                destY += 12;
                index++;
            }

        }
    }
}
