using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Physics
{
    [Component("physical")]
    class Physical : Component
    {
        public Vector2D Position
        {
            get => Owner.Components.Get<Spatial>().Position;
            set => Owner.Components.Get<Spatial>().Position = value;
        }
        public Vector2D Velocity = new Vector2D();

        public Vector2D PreviousPosition;
        public Vector2D PreviousVelocity;

        public double GravityMultiplier { get; set; } = 1;
    }
}
