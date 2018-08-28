using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Scenes
{
    public class CameraView
    {
        public Vector2D Location;
        public double X
        {
            get => Location.X;
            set => Location.X = value;
        }
        public double Y
        {
            get => Location.Y;
            set => Location.Y = value;
        }
        private float _scale = 1;
        public float Scale
        {
            get => _scale;
            set
            {
                if (value > 0) _scale = value;
            }
        }
    }
}
