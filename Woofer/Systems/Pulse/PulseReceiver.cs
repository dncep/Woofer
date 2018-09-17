using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Pulse
{
    [Component("pulse_receiver")]
    class PulseReceiver : Component
    {
        [PersistentProperty]
        public Vector2D Offset { get; set; }

        [PersistentProperty]
        public double Sensitivity { get; set; } = 1;
    }

    [Component("pulse_receiver_physical")]
    class PulseReceiverPhysical : Component
    {
    }
}
