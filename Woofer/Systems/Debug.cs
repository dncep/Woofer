using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using WooferGame.Systems.Camera.Shake;

namespace WooferGame.Systems
{
    [ComponentSystem("debug")]
    class DebugSystem : ComponentSystem
    {
        public DebugSystem()
        {
            InputProcessing = true;
        }

        private bool enabled = true;

        public override void Input()
        {
            if(enabled && (Woofer.Controller.InputManager.ActiveInputMap.Movement + Vector2D.UnitJ).Magnitude < 0.1)
            {
                Owner.Events.InvokeEvent(new CameraShakeEvent(null, 32));
                enabled = false;
            }
        }
    }
}
