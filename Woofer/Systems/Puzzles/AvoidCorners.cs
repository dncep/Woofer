using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Puzzles
{
    [Component("avoid_corners")]
    class AvoidCorners : Component
    {
        public float RecordedMass;
        public bool Bouncing;
    }
}
