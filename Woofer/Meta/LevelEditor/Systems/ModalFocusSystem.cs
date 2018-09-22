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

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("ModalFocusSystem", ProcessingCycles.Input, ProcessingFlags.Pause)]
    class ModalFocusSystem : ComponentSystem
    {
        private readonly InputTimeframe modalChange = new InputTimeframe(2);
        private string CurrentSystem = null;

        public override void Input()
        {
            ButtonState changeButton = Woofer.Controller.InputManager.ActiveInputMap.Back;
            modalChange.RegisterState(changeButton);

            if(changeButton.IsPressed() && modalChange.Execute())
            {
                BeginModalChangeEvent begin = new BeginModalChangeEvent(null);
                Owner.Events.InvokeEvent(begin);
                if(begin.SystemName != null)
                {
                    CurrentSystem = begin.SystemName;
                    Owner.Systems[CurrentSystem].EventFired(this, new ModalChangeEvent(null));
                }
            }
        }
    }
}
