using System;

using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
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

                //Jump logic
                if (jumpButton.IsPressed()) pmc.Jump.RegisterPressed();
                else pmc.Jump.RegisterUnpressed();
                if (pmc.OnGround)
                {
                    if(jumpButton.IsPressed() && pmc.Jump.Execute())
                    {
                        rb.Velocity.Y = pmc.JumpSpeed;
                    }
                }

                //Walking logic
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

                //For smoothly walking down slopes
                if(rb.Velocity.Y <= 0)
                {
                    //Raycast down under the player's feet up to a distance of 4 pixels
                    RaycastEvent raycast = new RaycastEvent(pmc, new FreeVector2D(rb.Position, rb.Position - 4 * Vector2D.UnitJ));
                    Owner.Events.InvokeEvent(raycast);
                    foreach(RaycastIntersection intersection in raycast.Intersected)
                    {
                        if(intersection.Component.Owner != pmc.Owner)
                        {
                            if(intersection.FaceProperties.Snap) //If the intersected face has the 'snap' property enabled (for slopes)
                            {
                                rb.Position = intersection.Point; //Move the player down to the point of intersection, assuming the player's origin is at their feet
                            }
                        }
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
