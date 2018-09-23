using System;
using System.Collections.Generic;
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

            StartInput.RegisterState(inputMap.Start);
            

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


            if (inputMap.Start.IsPressed() && StartInput.Execute())
            {
                switch (SelectedIndex)
                {
                    case 0:
                        Woofer.Controller.CommandFired(new SceneChangeCommand(new IntroScene()));
                        break;
                    case 1:
                        break;
                    case 2:
                        Woofer.Controller.CommandFired(new SceneChangeCommand(new Editor()));
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

            layer.Draw(r.SpriteManager["brick"], new System.Drawing.Rectangle(50, 50, 80, 80));

            int destY = 100;
            int index = 0;
            foreach(string label in OptionLabels)
            {
                TextUnit text;
                if (index == SelectedIndex) text = new TextUnit(new Sprite("gui", new Rectangle(0, 0, 8, 8), new Rectangle(0, 0, 8, 8)), label);
                else text = new TextUnit(label);
                text.Render(r, layer, new System.Drawing.Point(8, destY));
                destY += 12;
                index++;
            }

        }
    }
}
