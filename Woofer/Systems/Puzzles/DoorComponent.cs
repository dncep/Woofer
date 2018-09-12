using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Puzzles
{
    [Component("door")]
    class DoorComponent : Component
    {
        public double MaxOpenDistance = 48 - 8;
        public double CurrentOpenDistance = 0;
        public double OpeningTime = 0.5;

        public double OpeningDirection = 0;

        public bool Toggle = false;
    }
}
