using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [Event("camera_response")]
    class CameraLocationResponseEvent : Event
    {
        public Vector2D Location;
        public bool Overwrite;

        public CameraLocationResponseEvent(Component sender, Vector2D position) : this(sender, position, false)
        {
        }

        public CameraLocationResponseEvent(Component sender, Vector2D position, bool overwrite) : base(sender)
        {
            this.Location = position;
            this.Overwrite = overwrite;
        }
    }
}
