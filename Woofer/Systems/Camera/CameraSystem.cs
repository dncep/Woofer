using System.Linq;

using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [ComponentSystem("camera_system", ProcessingCycles.Input | ProcessingCycles.Tick),
        Watching(typeof(CameraTracked)),
        Listening(typeof(CameraLocationQueryEvent), typeof(CameraLocationResponseEvent))]
    class CameraSystem : ComponentSystem
    {
        private Vector2D location;

        public override void Input()
        {
            Owner.CurrentViewport.Location += Woofer.Controller.InputManager.ActiveInputMap.Movement;
        }

        public override void Tick()
        {
            location = Owner.CurrentViewport.Location;
            
            Owner.Events.InvokeEvent(new CameraLocationQueryEvent(null));

            Owner.CurrentViewport.Location = location;
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is CameraLocationQueryEvent qe && WatchedComponents.Count > 0)
            {
                Owner.Events.InvokeEvent(new CameraLocationResponseEvent(null,
                    WatchedComponents.First().Owner.Components.Get<Spatial>().Position + 
                    (WatchedComponents.First() as CameraTracked).Offset,
                    true
                    ));
            }
            else if (e is CameraLocationResponseEvent re)
            {
                if (re.Overwrite) location = re.Location;
                else location += re.Location;
            }
        }
    }
}
