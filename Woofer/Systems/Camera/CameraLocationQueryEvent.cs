using EntityComponentSystem.Components;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Camera
{
    [Event("camera_query")]
    class CameraLocationQueryEvent : Event
    {
        public CameraLocationQueryEvent(Component sender) : base(sender)
        {
        }
    }
}
