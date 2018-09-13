using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Puzzles
{
    [Component("switch")]
    class SwitchComponent : Component
    {
        public int PressedState = 0;
        public bool Pressed => PressedState > 0;
    }
}
