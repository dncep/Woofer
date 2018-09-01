using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using GameBase;

namespace WooferGame.Systems.Camera
{
    [ComponentSystem("camera_system")]
    class CameraSystem : ComponentSystem
    {
        public CameraSystem()
        {
            Watching = new string[] { Component.IdentifierOf<CameraTracked>() };
            InputProcessing = true;
            TickProcessing = true;
        }

        public override void Input()
        {
            Owner.CurrentViewport.Location += Woofer.Controller.InputUnit.GamePads[0].Thumbsticks.Right;
            if (WatchedComponents.Count > 0)
            {
                Owner.CurrentViewport.Location = WatchedComponents.First().Owner.Components.Get<Spatial>().Position + (WatchedComponents.First() as CameraTracked).Offset;
            }
        }

        public override void Tick()
        {
            
        }
    }
}
