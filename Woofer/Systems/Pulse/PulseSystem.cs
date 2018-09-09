using System;
using System.Linq;

using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

using GameInterfaces.Input;

using WooferGame.Input;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;
using WooferGame.Systems.Player.Actions;

namespace WooferGame.Systems.Pulse
{
    [ComponentSystem("pulse_system", 
        ProcessingCycles.Input | ProcessingCycles.Tick),
        Watching(typeof(PulseAbility), typeof(PulsePushable), typeof(PulseReceiver)),
        Listening(typeof(PulseEvent))]
    class PulseSystem : ComponentSystem
    {
        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            foreach(PulseAbility pa in WatchedComponents.Where(c => c is PulseAbility))
            {
                ButtonState pulseButton = inputMap.Pulse;

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
                    if (!pp.Owner.Active) continue;
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
                        ph.Velocity += ((center - e.Source).Unit() * ((e.Reach - distance) * e.Strength/2) / mass);
                    }
                }

                foreach(PulseReceiver pr in WatchedComponents.Where(c => c is PulseReceiver))
                {
                    if (!pr.Owner.Active) return;
                    if (pr.Owner == re.Sender.Owner) continue;

                    Vector2D point = pr.Offset;

                    if (pr.Owner.Components.Get<Spatial>() is Spatial sp) point += sp.Position;

                    if(e.Source.Magnitude == 0 || GeneralUtil.SubtractAngles((point - e.Source).Angle, e.Direction.Angle) <= Math.PI/4)
                    {
                        double distance = (point - e.Source).Magnitude;
                        if(distance <= e.Reach * pr.Sensitivity)
                        {
                            Owner.Events.InvokeEvent(new ActivationEvent(e.Sender, pr.Owner, e));
                        }
                    }
                }
            }
        }
    }
}
