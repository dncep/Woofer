using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using WooferGame.Input;

namespace WooferGame.Systems.Debug
{
    [Component("debug_clippable")]
    class DebugClippable : Component
    {
        [PersistentProperty]
        public bool Enabled { get; set; } = false;
        [PersistentProperty]
        public Vector2D CameraLocation { get; set; }
        public InputTimeframe Toggle = new InputTimeframe(8);
    }
}
