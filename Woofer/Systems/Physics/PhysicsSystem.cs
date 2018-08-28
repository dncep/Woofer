using System;
using System.Collections.Generic;
using System.Linq;

using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;

namespace Woofer.Systems.Physics
{
    public class PhysicsSystem : ComponentSystem
    {
        public const string Identifier = "physics";
        private Vector2D Gravity = new Vector2D(0, -192);

        public PhysicsSystem()
        {
            SystemName = Identifier;
            Watching = new string[] { RectangleBody.Identifier };
            TickProcessing = true;
        }

        private float accumulator = 0.0f;

        public override void Tick()
        {
            if (Owner.FixedDeltaTime == 0) return;
            accumulator += Owner.DeltaTime;

            while (accumulator >= Owner.FixedDeltaTime)
            {
                accumulator -= Owner.FixedDeltaTime;
                foreach (RectangleBody rb in WatchedComponents)
                {
                    rb.PreviousPosition = rb.Position;
                    rb.PreviousVelocity = rb.Velocity;

                    if (!rb.Immovable) rb.Velocity += Gravity * Owner.FixedDeltaTime;
                    rb.Position += rb.Velocity * Owner.FixedDeltaTime;
                }

                WatchedComponents.Sort((a, b) =>
                {
                    return (int)((GetCrossTickLeft(a as RectangleBody) - GetCrossTickLeft(b as RectangleBody)) * 1000);
                });

                List<RectangleBody> sweeper = new List<RectangleBody>();

                //Handle collision
                foreach (RectangleBody rb0 in WatchedComponents)
                {
                    double x = GetCrossTickLeft(rb0);

                    while (sweeper.Count > 0 && !IntersectsX(sweeper[0], x))
                    {
                        sweeper.RemoveAt(0);
                    }

                    foreach(RectangleBody rb1 in sweeper)
                    {
                        if (rb0.Immovable && rb1.Immovable) continue;
                        RectangleBody rb = !rb0.Immovable ? rb0 : rb1;
                        RectangleBody other = rb == rb0 ? rb1 : rb0;

                        BoundingBox intersection = rb.RealBounds.Intersect(other.RealBounds);
                        if (intersection != null)
                        {
                            if (other.Immovable) //Hard collision
                            {
                                Vector2D totalVelocity = rb.Velocity - other.Velocity;

                                List<FreeVector2D> sides = intersection.GetSides().Where(s => GeneralUtil.SubtractAngles(s.Normal.Angle, totalVelocity.Angle) > Math.PI / 2).Where(s => BelongsToBox(s, other.RealBounds)).ToList();

                                if(sides.Count == 0) //It's stuck inside a tile
                                {
                                    //Push upwards
                                    rb.Position += new Vector2D(0, 1);
                                    rb.Velocity += new Vector2D(0, 16);
                                    continue;
                                }

                                Vector2D normal = sides[0].Normal;
                                if (sides.Count > 1)
                                {
                                    normal = IsAheadOfNormal(rb.Bounds.Offset(rb.PreviousPosition), sides[0]) ? sides[0].Normal : sides[1].Normal;
                                }

                                if(normal.X == 0)
                                {
                                    rb.Position += new Vector2D(0, intersection.Height) * normal.Y;
                                    rb.Velocity = new Vector2D(rb.Velocity.X * (1-other.Friction), 0);
                                } else
                                {
                                    rb.Position += new Vector2D(intersection.Width, 0) * normal.X;
                                    rb.Velocity = new Vector2D(0, rb.Velocity.Y * (1-other.Friction));
                                }
                                //off--;
                            } else //Soft Collision
                            {

                            }
                        }


                    }

                    sweeper.Add(rb0);
                }
            }
        }

        public static bool IsAheadOfNormal(BoundingBox box, FreeVector2D side)
        {
            Vector2D normal = side.Normal;
            if(normal.Y == 0)
            {
                return normal.X > 0 ? box.Left > side.A.X : box.Right < side.A.X;
            } else
            {
                return normal.Y > 0 ? box.Bottom > side.A.Y : box.Top < side.A.Y;
            }
        }

        public static  bool BelongsToBox(FreeVector2D side, BoundingBox realBounds)
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

        private bool IntersectsX(RectangleBody rb, double x)
        {
            return GetCrossTickLeft(rb) <= x && x <= GetCrossTickRight(rb);
        }

        private double GetCrossTickLeft(RectangleBody rb)
        {
            return Math.Min(
                rb.Bounds.Offset(rb.Position).Left,
                rb.Bounds.Offset(rb.PreviousPosition).Left
            );
        }

        private double GetCrossTickRight(RectangleBody rb)
        {
            return Math.Max(
                rb.Bounds.Offset(rb.Position).Right,
                rb.Bounds.Offset(rb.PreviousPosition).Right
            );
        }
    }
}
