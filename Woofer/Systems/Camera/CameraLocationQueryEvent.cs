using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
