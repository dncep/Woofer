using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [Component("camera_tracked")]
    class CameraTracked : Component
    {
        public Vector2D Offset { get; set; }

        public CameraTracked()
        {
        }

    }
}
