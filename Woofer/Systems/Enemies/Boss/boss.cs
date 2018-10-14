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

namespace WooferGame.Systems.Enemies.Boss
{
    [Component("boss")]
    class Boss : Component
    {
        public const int Circling = 1;
        public const int Laser = 2;


        public double HoverTimer { get; set; }
        public int State { get; set; } = Circling;
        public bool Transitioning { get; set; } = true;

        public double StateData { get; set; }

        public void ChangeState(int state)
        {
            State = state;
            Transitioning = true;
            StateData = 0;
        }
    }
}
