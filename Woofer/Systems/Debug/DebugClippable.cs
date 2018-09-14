using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using WooferGame.Input;

namespace WooferGame.Systems.Debug
{
    [Component("debug_clippable")]
    class DebugClippable : Component
    {
        public bool Enabled = false;
        public Vector2D CameraLocation;
        public InputTimeframe Toggle = new InputTimeframe(8);
    }
}
