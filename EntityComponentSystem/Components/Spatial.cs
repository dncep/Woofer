using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Components
{
    public class Spatial : Component
    {
        public const string Identifier = "spatial";

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

        public Spatial() => ComponentName = Identifier;

        public Spatial(float X, float Y) : this()
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
