using System;

using EntityComponentSystem.Components;

using WooferGame.Input;

namespace WooferGame.Systems.Player.Actions
{
    [Component("pulse_ability")]
    class PulseAbility : Component
    {
        public double MaxEnergy = 100;
        public double EnergyMeter = 100;

        public double PulseCost = 20;
        public double PulseStrength = 256;

        public double MaxArc = Math.PI / 6;
        public double MaxRange = 48;

        public InputTimeframe Pulse = new InputTimeframe(5);
    }
}
