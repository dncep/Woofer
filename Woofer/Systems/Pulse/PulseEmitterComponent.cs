using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Pulse
{
    [Component("pulse_emitter")]
    class PulseEmitterComponent : Component
    {
        public Vector2D Direction;
        public double Strength;
        public double Reach;

        public PulseEmitterComponent(Vector2D direction, double strength, double reach)
        {
            Direction = direction;
            Strength = strength;
            Reach = reach;
        }

        public Vector2D Offset { get; internal set; }
    }
}
