using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using GameBase;
using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Player.Actions
{
    [ComponentSystem("pulse_system")]
    class PulseSystem : ComponentSystem
    {
        public PulseSystem()
        {
            InputProcessing = true;
            TickProcessing = true;
            Watching = new string[] { Component.IdentifierOf<PulseAbility>() };
        }

        public override void Input()
        {
            IGamePad gamePad = Woofer.Controller.InputUnit.GamePads[0];

            foreach(PulseAbility pa in WatchedComponents)
            {
                ButtonState pulseButton = gamePad.Buttons.X;

                if (pulseButton.IsPressed()) pa.Pulse.RegisterPressed();
                else pa.Pulse.RegisterUnpressed();

                Console.WriteLine($"{pa.EnergyMeter}/{pa.MaxEnergy}");

                if(pa.EnergyMeter >= pa.PulseCost)
                {
                    if(pulseButton.IsPressed() && pa.Pulse.Execute())
                    {
                        pa.Owner.Components.Get<RectangleBody>().Velocity = pa.Owner.Components.Get<PlayerOrientation>().Unit * -(pa.PulseStrength * Math.Sqrt(pa.EnergyMeter / pa.MaxEnergy));
                        pa.EnergyMeter -= pa.PulseCost;
                    }
                }
            }
        }
        public override void Tick()
        {
            foreach(PulseAbility pa in WatchedComponents)
            {
                if (pa.Owner.Components.Get<PlayerMovementComponent>().OnGround) pa.EnergyMeter = pa.MaxEnergy;
                Console.WriteLine(pa.EnergyMeter);
                pa.Pulse.Tick();
            }
        }
    }
}
