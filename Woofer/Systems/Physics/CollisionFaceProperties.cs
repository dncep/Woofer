﻿using System;
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
        public bool Snap;

        public CollisionFaceProperties(bool enabled, double friction)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = false;
        }

        public CollisionFaceProperties(bool enabled, double friction, bool snap)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = snap;
        }

        public override string ToString() => $"[Enabled: {Enabled}, Friction: {Friction}]";
    }
}
