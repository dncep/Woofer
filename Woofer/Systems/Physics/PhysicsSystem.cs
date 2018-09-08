using System;
using System.Collections.Generic;
using System.Linq;

using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Physics
{
    [ComponentSystem("physics", ProcessingCycles.Tick),
        Watching(typeof(Physical), typeof(RigidBody), typeof(SoftBody))]
    public class PhysicsSystem : ComponentSystem
    {
        private Vector2D Gravity = new Vector2D(0, -362);

        private float accumulator = 0.0f;

        public override void Tick()
        {
            if (Owner.FixedDeltaTime == 0) return;
            accumulator += Owner.DeltaTime;
            
            int timesExecuted = 0;
            
            while (accumulator >= Owner.FixedDeltaTime)
            {
                timesExecuted++;
                accumulator -= Owner.FixedDeltaTime;
                foreach (Physical ph in WatchedComponents.Where(c => c is Physical))
                {
                    if (!ph.Owner.Active) continue;
                    ph.PreviousPosition = ph.Position;

                    ph.Velocity += Gravity * ph.GravityMultiplier * Owner.FixedDeltaTime;
                    ph.Position += ph.Velocity * Owner.FixedDeltaTime;

                    ph.PreviousVelocity = ph.Velocity;
                }
                
                WatchedComponents = WatchedComponents.OrderBy(a => GetCrossTickLeft(a)).ToList();

                List<Component> sweeper = new List<Component>();

                //Handle collision
                foreach (Component c0 in WatchedComponents)
                {
                    if (c0 is Physical) continue;
                    if (!c0.Owner.Active) continue;
                    double x = GetCrossTickLeft(c0);

                    while (sweeper.Count > 0 && !IntersectsX(sweeper[0], x))
                    {
                        sweeper.RemoveAt(0);
                    }
                    
                    foreach (Component c1 in sweeper)
                    {
                        if (c0 is RigidBody && c1 is RigidBody) continue;

                        SoftBody objA = c0 is SoftBody ? (SoftBody) c0 : (SoftBody) c1;
                        Component objB = objA == c0 ? c1 : c0;

                        Physical physA = objA.Owner.Components.Get<Physical>();
                        Physical physB = objB.Owner.Components.Get<Physical>();

                        IEnumerable<CollisionBox> solidBoxes = 
                            objB is SoftBody ? 
                                new CollisionBox[] { (objB as SoftBody).Bounds.Offset(physB.Position) } : 
                                (objB as RigidBody).Bounds.Select(b => b.Offset(physB.Position));

                        foreach(CollisionBox otherBounds in solidBoxes)
                        {
                            CollisionBox intersection = objA.Bounds.Offset(physA.Position).Intersect(otherBounds);

                            if (intersection != null)
                            {
                                if (objB is RigidBody) //Hard collision
                                {
                                    Vector2D totalVelocity = physA.PreviousVelocity - physB.Velocity;

                                    List<FreeVector2D> sides = intersection.GetSides().Where(s => BelongsToBox(s, otherBounds)).Where(s => GeneralUtil.SubtractAngles(s.Normal.Angle, totalVelocity.Angle) > Math.PI / 2).ToList();

                                    if (sides.Count == 0) //It's "stuck" inside a tile or that tile doesn't have one of the sides enabled
                                    {
                                        continue;
                                    }

                                    FreeVector2D normalSide = sides[0];
                                    Vector2D normal = sides[0].Normal;

                                    if (sides.Count > 1)
                                    {
                                        if (IsAheadOfNormal(objA.Bounds.Offset(physA.PreviousPosition), sides[0]))
                                        {
                                            if (IsAheadOfNormal(objA.Bounds.Offset(physA.PreviousPosition), sides[1]))
                                            {
                                                normalSide = sides[0].Normal.X == 0 ? sides[0] : sides[1];
                                            }
                                            else normalSide = sides[0];
                                        }
                                        else normalSide = sides[1];

                                        normal = normalSide.Normal;
                                    }
                                    CollisionFaceProperties faceProperties = intersection.GetFaceProperties(normal);

                                    if (normal.X == 0)
                                    {
                                        double displacement = Math.Abs(normalSide.A.Y - (normal.Y > 0 ? objA.Bounds.Offset(physA.Position).Bottom : objA.Bounds.Offset(physA.Position).Top));
                                        if (faceProperties.Snap || Math.Round(displacement, 8) <= Math.Round(Math.Abs(physA.Position.Y - physA.PreviousPosition.Y), 8))
                                        {
                                            physA.Position += new Vector2D(0, displacement) * normal.Y;
                                            physA.Velocity = new Vector2D(physA.Velocity.X * (1 - faceProperties.Friction), 0) + physB.Velocity * faceProperties.Friction;
                                        }
                                        else continue;
                                    }
                                    else
                                    {
                                        double displacement = Math.Abs(normalSide.A.X - (normal.X > 0 ? objA.Bounds.Offset(physA.Position).Left : objA.Bounds.Offset(physA.Position).Right));
                                        if (faceProperties.Snap || Math.Round(displacement, 8) <= Math.Round(Math.Abs(physA.Position.X - physA.PreviousPosition.X), 8))
                                        {
                                            physA.Position += new Vector2D(displacement, 0) * normal.X;
                                            physA.Velocity = new Vector2D(0, physA.Velocity.Y * (1 - faceProperties.Friction)) + physB.Velocity * faceProperties.Friction;
                                        }
                                        else continue;
                                    }

                                    Owner.Events.InvokeEvent(new CollisionEvent(objA, objB.Owner, normal));
                                    Owner.Events.InvokeEvent(new CollisionEvent(objB, objA.Owner, normal));
                                }
                                else //Soft Collision
                                {
                                    Vector2D center0 = objA.Bounds.Offset(physA.Position).Center;
                                    Vector2D center1 = otherBounds.Center;

                                    double distance = (center0 - center1).Magnitude;
                                    if (distance <= 1e-4) continue;

                                    double force = 2 * Owner.FixedDeltaTime * intersection.Area * (objA.Mass + (objB as SoftBody).Mass);

                                    Vector2D forceVec = (center1 - center0).Unit() * force;
                                    forceVec.Y = 0;

                                    physA.Velocity -= forceVec / objA.Mass;
                                    physB.Velocity -= -forceVec / (objB as SoftBody).Mass;
                                }
                            }
                        }


                    }

                    sweeper.Add(c0);
                }
            }
        }

        public static bool IsAheadOfNormal(CollisionBox box, FreeVector2D side)
        {
            Vector2D normal = side.Normal;
            if(normal.Y == 0)
            {
                return normal.X > 0 ? box.Left >= side.A.X : box.Right <= side.A.X;
            } else
            {
                return normal.Y > 0 ? box.Bottom >= side.A.Y : box.Top <= side.A.Y;
            }
        }

        public static bool BelongsToBox(FreeVector2D side, CollisionBox realBounds)
        {
            if(side.A.Y == side.B.Y) //Is horizontal
            {
                return side.A.Y == realBounds.Bottom || side.A.Y == realBounds.Top;
            }
            else if(side.A.X == side.B.X) //Is vertical
            {
                return side.A.X == realBounds.Left || side.B.X == realBounds.Right;
            }
            throw new ArgumentException("Side given is not horizontal nor vertical");
        }

        private bool IntersectsX(Component c, double x)
        {
            return GetCrossTickLeft(c) <= x && x <= GetCrossTickRight(c);
        }

        private double GetCrossTickLeft(Component c)
        {
            if (c is Physical ph) return Math.Min(ph.Position.X, ph.PreviousPosition.X);
            ph = c.Owner.Components.Get<Physical>();
            SoftBody sb = c as SoftBody;

            CollisionBox box = (c is RigidBody rb) ? rb.UnionBounds : sb?.Bounds;

            return Math.Min(
                box.Offset(ph.Position).Left,
                box.Offset(ph.PreviousPosition).Left
            );
        }

        private double GetCrossTickRight(Component c)
        {
            if (c is Physical ph) return Math.Min(ph.Position.X, ph.PreviousPosition.X);
            ph = c.Owner.Components.Get<Physical>();
            SoftBody sb = c as SoftBody;

            CollisionBox box = (c is RigidBody rb) ? rb.UnionBounds : sb?.Bounds;

            return Math.Max(
                box.Offset(ph.Position).Right,
                box.Offset(ph.PreviousPosition).Right
            );
        }
    }
}
