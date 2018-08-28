using EntityComponentSystem.Scenes;
using GameInterfaces.Controller;
using Woofer.Scenes;

namespace Woofer.Controller
{
    public class WooferController : IGameController
    {
        public Scene ActiveScene { get; internal set; }
        public IRenderingUnit RenderingUnit { get; internal set; }
        public IInputUnit InputUnit { get; set; }

        public void Initialize() {}

        public WooferController()
        {
            RenderingUnit = new WooferRenderingUnit(this);
            ActiveScene = new TestScene();
        }
    }
}
