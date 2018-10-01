using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Pulse
{
    [Component("pulse_emitter")]
    class PulseEmitterComponent : Component
    {
        [PersistentProperty]
        public Vector2D Direction;
        [PersistentProperty]
        public float Strength;
        [PersistentProperty]
        public float Reach;

        [PersistentProperty]
        [Inspector(InspectorEditType.Offset)]
        public Vector2D Offset { get; set; }

        public PulseEmitterComponent()
        {
        }

        public PulseEmitterComponent(Vector2D direction, float strength, float reach)
        {
            Direction = direction;
            Strength = strength;
            Reach = reach;
        }

    }
}
