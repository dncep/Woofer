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
