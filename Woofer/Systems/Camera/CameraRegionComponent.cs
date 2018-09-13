using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera
{
    [Component("camera_region")]
    class CameraRegionComponent : Component
    {
        public Rectangle Area;
        public Vector2D Focus;

        public double Easing = 0;

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
