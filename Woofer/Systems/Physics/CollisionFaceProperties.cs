using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Physics
{
    public struct CollisionFaceProperties
    {
        public bool Enabled;
        public double Friction;

        public CollisionFaceProperties(bool enabled, double friction)
        {
            Enabled = enabled;
            Friction = friction;
        }

        public override string ToString() => $"[Enabled: {Enabled}, Friction: {Friction}]";
    }
}
