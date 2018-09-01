using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Components
{
    [Component("spatial")]
    public class Spatial : Component
    {
        private Vector2D position = new Vector2D();
        public Vector2D Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public double X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }
        public double Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public Spatial() { }

        public Spatial(float X, float Y) : this()
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
