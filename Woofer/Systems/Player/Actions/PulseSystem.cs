using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using GameBase;
using GameInterfaces.Input;
using GameInterfaces.Input.GamePad;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Pulse;

namespace WooferGame.Systems.Player.Actions
{
    [ComponentSystem("pulse_system")]
    class PulseSystem : ComponentSystem
    {
        public PulseSystem()
        {
            InputProcessing = true;
            TickProcessing = true;
            Watching = new string[] { Component.IdentifierOf<PulseAbility>(), Component.IdentifierOf<PulsePushable>() };
            Listening = new string[] { Event.IdentifierOf<PulseEvent>() };
        }

        public override void Input()
        {
            IGamePad gamePad = Woofer.Controller.InputUnit.GamePads[0];

            foreach(PulseAbility pa in WatchedComponents.Where(c => c is PulseAbility))
            {
                ButtonState pulseButton = gamePad.Buttons.X;

                if (pulseButton.IsPressed()) pa.Pulse.RegisterPressed();
                else pa.Pulse.RegisterUnpressed();

                if(pa.EnergyMeter >= pa.PulseCost)
                {
                    if(pulseButton.IsPressed() && pa.Pulse.Execute())
                    {
                        double strength = (pa.PulseStrength * Math.Sqrt(pa.EnergyMeter / pa.MaxEnergy));

                        if (pa.Owner.Components.Has<Physical>() && pa.Owner.Components.Has<PlayerOrientation>())
                        {
                            pa.Owner.Components.Get<Physical>().Velocity = pa.Owner.Components.Get<PlayerOrientation>().Unit * -strength;
                        }

                        pa.EnergyMeter -= pa.PulseCost;

                        if (pa.Owner.Components.Has<Spatial>()) {

                            Spatial sp = pa.Owner.Components.Get<Spatial>();
                            if (sp == null) continue;
                            PlayerOrientation po = pa.Owner.Components.Get<PlayerOrientation>();

                            Owner.Events.InvokeEvent(new PulseEvent(pa, sp.Position, po != null ? po.Unit : new Vector2D(), strength, pa.MaxRange));
                        }
                    }
                }
            }
        }

        public override void Tick()
        {
            foreach(PulseAbility pa in WatchedComponents.Where(c => c is PulseAbility))
            {
                if (pa.Owner.Components.Get<PlayerMovementComponent>().OnGround) pa.EnergyMeter = pa.MaxEnergy;
                pa.Pulse.Tick();
            }
        }

        public override void EventFired(object sender, Event re)
        {
            if(re is PulseEvent)
            {
                PulseEvent e = re as PulseEvent;

                foreach(PulsePushable pp in WatchedComponents.Where(c => c is PulsePushable))
                {
                    if (pp.Owner == re.Sender.Owner) continue;
                    if (!pp.Owner.Components.Has<Spatial>() || !pp.Owner.Components.Has<Physical>()) continue;
                    Physical ph = pp.Owner.Components.Get<Physical>();

                    Vector2D center = pp.Owner.Components.Has<SoftBody>() ? pp.Owner.Components.Get<SoftBody>().Bounds.Offset(ph.Position).Center : ph.Position;

                    double distance = (center - e.Source).Magnitude;

                    if (distance > e.Reach) continue;

                    if(e.Source.Magnitude == 0 || GeneralUtil.SubtractAngles((center - e.Source).Angle, e.Direction.Angle) <= Math.PI/4)
                    {
                        double mass = 1;
                        if (pp.Owner.Components.Has<SoftBody>()) mass = pp.Owner.Components.Get<SoftBody>().Mass;
                        ph.Velocity += ((center - e.Source).Unit() * ((e.Reach - distance) * e.Strength/8) / mass);
                    }
                }
            }
        }
    }
}
