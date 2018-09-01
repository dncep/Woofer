using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Player
{
    [Component("player_orientation")]
    class PlayerOrientation : Component
    {
        public Vector2D Unit { get; set; }
        public double Angle => Unit.Angle;
    }
}
