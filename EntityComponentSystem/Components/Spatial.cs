using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Components
{
    [Component("spatial")]
    public class Spatial : Component
    {
        [PersistentProperty("pos")]
        public Vector2D Position { get; set; } = new Vector2D();

        public double X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position = new Vector2D(value, Position.Y);
            }
        }
        public double Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position = new Vector2D(Position.X, value);
            }
        }

        public Spatial() { }

        public Spatial(double X, double Y) : this()
        {
            this.X = X;
            this.Y = Y;
        }

        public Spatial(Vector2D position)
        {
            this.Position = position;
        }
    }
}
