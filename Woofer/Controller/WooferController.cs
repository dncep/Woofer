using EntityComponentSystem.Scenes;
using GameInterfaces.Controller;
using WooferGame.Input;
using WooferGame.Scenes;

namespace WooferGame.Controller
{
    class WooferController : IGameController
    {
        public Scene ActiveScene { get; internal set; }
        public IRenderingUnit RenderingUnit { get; internal set; }
        public IInputUnit InputUnit { get; set; }

        public IInputMap InputMap { get; private set; }

        public void Initialize() {
            InputMap = new KeyboardInputMap(InputUnit.Keyboard, InputUnit.Mouse);
        }

        public WooferController()
        {
            RenderingUnit = new WooferRenderingUnit(this);
            ActiveScene = new TestScene();
        }
    }
}
