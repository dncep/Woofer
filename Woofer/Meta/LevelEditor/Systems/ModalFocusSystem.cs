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
    [ComponentSystem("ModalFocusSystem", ProcessingCycles.Input, ProcessingFlags.Pause),
        Listening(typeof(ForceModalChangeEvent))]
    class ModalFocusSystem : ComponentSystem
    {
        private readonly InputTimeframe modalChange = new InputTimeframe(2);
        private string CurrentSystem = "editor_cursor";

        public override void Input()
        {
            ButtonState changeButton = Woofer.Controller.InputManager.ActiveInputMap.Back;
            modalChange.RegisterState(changeButton);

            if(changeButton.IsPressed() && modalChange.Execute())
            {
                BeginModalChangeEvent begin = new BeginModalChangeEvent(null);
                Owner.Systems[CurrentSystem].EventFired(this, begin);
                Owner.Events.InvokeEvent(begin);
                if(begin.SystemName != null)
                {
                    ChangeSystem(begin.SystemName);
                }
            }
        }

        private void ChangeSystem(string name)
        {
            Owner.Systems[name].EventFired(this, new ModalChangeEvent(CurrentSystem, null));
            CurrentSystem = name;
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is ForceModalChangeEvent ce)
            {
                ChangeSystem(ce.SystemName);
            }
        }
    }
}
