using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using GameInterfaces.Controller;
using Point = System.Drawing.Point;
using Color = System.Drawing.Color;
using WooferGame.Controller.Commands;
using WooferGame.Scenes;

namespace WooferGame.Meta.FileSelect
{
    [ComponentSystem("file_select", ProcessingCycles.Input | ProcessingCycles.Render)]
    class FileSelectSystem : ComponentSystem
    {
        public override void Input()
        {
            if(Woofer.Controller.InputManager.ActiveInputMap.Back.Consume())
            {
                Woofer.Controller.CommandFired(new DirectSceneChangeCommand(new MainMenuScene()));
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var bg = r.GetLayerGraphics("hud");

            bg.Draw(r.SpriteManager["menu_bg"], new System.Drawing.Rectangle(new Point(0, 0), bg.GetSize()));

            var layer = r.GetLayerGraphics("hi_res_overlay");

            layer.FillRect(new System.Drawing.Rectangle(0, 300, layer.GetSize().Width, layer.GetSize().Height), Color.FromArgb(127, 0, 0, 0));
        }
    }
}
