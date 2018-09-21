using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using GameInterfaces.Input;
using WooferGame.Input;

namespace WooferGame.Systems.Menu
{
    [ComponentSystem("pausing", ProcessingCycles.Input, ProcessingFlags.Pause)]
    class PauseSystem : ComponentSystem
    {
        private readonly InputTimeframe Pause = new InputTimeframe(1);

        public override void Input()
        {
            ButtonState state = Woofer.Controller.InputManager.ActiveInputMap.Pause;
            Pause.RegisterState(state);
            if(state.IsPressed() && Pause.Execute())
            {
                Woofer.Controller.Paused = !Woofer.Controller.Paused;
            }
        }
    }
}
