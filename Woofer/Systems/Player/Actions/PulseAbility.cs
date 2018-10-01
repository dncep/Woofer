using System;

using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Player.Actions
{
    [Component("pulse_ability")]
    class PulseAbility : Component
    {
        [PersistentProperty]
        public float MaxEnergy = 100;

        [PersistentProperty]
        public float EnergyMeter = 100;


        [PersistentProperty]
        public float PulseCost = 20;

        [PersistentProperty]
        public float PulseStrength = 256;


        [PersistentProperty]
        public float MaxRange = 48;


        [PersistentProperty]
        [Inspector(InspectorEditType.Offset)]
        public Vector2D Offset { get; set; } = new Vector2D(0, 16);
    }
}
