using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Camera
{
    [Component("camera_region")]
    class CameraRegionComponent : Component
    {
        [PersistentProperty]
        public Rectangle Area { get; set; }
        [PersistentProperty]
        [Inspector(InspectorEditType.Offset)]
        public Vector2D Focus { get; set; }

        [PersistentProperty]
        public float Easing { get; set; } = 0;
        [PersistentProperty]
        public float EasingStep { get; set; } = 0.01f;
        [PersistentProperty]
        public float MaxEasing { get; set; } = 1;

        public CameraRegionComponent()
        {
        }

        public CameraRegionComponent(Rectangle area) : this(area, area.Center)
        {

        }

        public CameraRegionComponent(Rectangle area, Vector2D focus)
        {
            this.Area = area;
            this.Focus = focus;
        }
    }
}
