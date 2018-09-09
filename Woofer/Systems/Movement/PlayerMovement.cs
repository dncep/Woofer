using System;

using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;

using GameInterfaces.Input;

using WooferGame.Input;
using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Movement
{
    [ComponentSystem("player_controller", ProcessingCycles.Input | ProcessingCycles.Tick),
        Watching(typeof(PlayerMovementComponent)),
        Listening(typeof(RigidCollisionEvent))]
    class PlayerMovement : ComponentSystem
    {
        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            foreach(PlayerMovementComponent pmc in WatchedComponents)
            {
                Physical rb = pmc.Owner.Components.Get<Physical>();

                ButtonState jumpButton = inputMap.Jump;

                if (jumpButton.IsPressed()) pmc.Jump.RegisterPressed();
                else pmc.Jump.RegisterUnpressed();

                if (pmc.OnGround)
                {
                    if(jumpButton.IsPressed() && pmc.Jump.Execute())
                    {
                        rb.Velocity.Y = pmc.JumpSpeed;
                    }
                }

                double xMovement = inputMap.Movement.X * pmc.CurrentSpeed;
                double xMovementCap = pmc.CurrentMaxSpeed;

                if(xMovement > 0)
                {
                    if(rb.Velocity.X <= xMovementCap)
                    {
                        rb.Velocity.X = Math.Min(rb.Velocity.X + xMovement, xMovementCap);
                    }
                } else if(xMovement < 0)
                {
                    if (rb.Velocity.X >= -xMovementCap)
                    {
                        rb.Velocity.X = Math.Max(rb.Velocity.X + xMovement, -xMovementCap);
                    }
                }
            }
        }

        public override void EventFired(object sender, Event re)
        {
            if(re is RigidCollisionEvent)
            {
                RigidCollisionEvent e = re as RigidCollisionEvent;

                if(e.Victim.Components.Has<PlayerMovementComponent>() && e.Normal.Y > 0)
                {
                    PlayerMovementComponent pmc = e.Victim.Components.Get<PlayerMovementComponent>();
                    pmc.OnGround = true;
                }
            }
        }

        public override void Tick()
        {
            foreach(PlayerMovementComponent pmc in WatchedComponents)
            {
                pmc.OnGround = false;
                pmc.Jump.Tick();
            }
        }
    }
}
