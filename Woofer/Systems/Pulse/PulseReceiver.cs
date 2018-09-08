using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Pulse
{
    [Component("pulse_receiver")]
    class PulseReceiver : Component
    {
        public Vector2D Offset { get; set; }
        public double Sensitivity { get; set; } = 1;
    }
}
