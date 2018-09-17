using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [Component("camera_tracked")]
    class CameraTracked : Component
    {
        [PersistentProperty]
        public Vector2D Offset { get; set; }

        public CameraTracked()
        {
        }
    }
}
