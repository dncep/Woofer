using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using WooferGame.Systems.Pulse;

namespace WooferGame.Systems.Player.Feedback
{
    [ComponentSystem("player_feedback", ProcessingCycles.Tick),
        Listening(typeof(PulseEvent))]
    class PlayerFeedbackSystem : ComponentSystem
    {
        private double VibrationAmount;

        public override void EventFired(object sender, Event evt)
        {
            if(evt is PulseEvent pe)
            {
                VibrationAmount += Math.Pow(pe.Strength/256, 2)*256;
            }
        }
        public override void Tick()
        {
            Woofer.Controller.InputManager.ActiveInputMap.SetVibration((float)(VibrationAmount/2));
            VibrationAmount = Math.Max(VibrationAmount/8, 0);
            if (VibrationAmount <= 1e-8) VibrationAmount = 0;
        }
    }
}
