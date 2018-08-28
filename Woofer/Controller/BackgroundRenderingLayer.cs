using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInterfaces.Controller;

namespace Woofer.Controller
{
    public class BackgroundRenderingLayer : IRenderingLayer
    {
        private IGameController gameController;

        public string Name => "background";
        public Size LayerSize => new Size(320, 180);
        public Rectangle Destination => new Rectangle(new Point(0, 0), gameController.RenderingUnit.ScreenSize);

        public BackgroundRenderingLayer(IGameController gameController) => this.gameController = gameController;
    }
}
