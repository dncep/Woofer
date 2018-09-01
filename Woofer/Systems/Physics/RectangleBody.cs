using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Physics
{
    [Component("rectanglebody")]
    class RectangleBody : Component
    {
        public CollisionBox Bounds { get; private set; }
        public float Mass { get; set; }
        public float Friction { get; set; } = 0.3f;
        public bool Immovable { get; set; }

        //Friction:
        // Ice-like: 0.01f;
        // Brick-like: 0.3f

        public CollisionBox RealBounds => Bounds.Offset(Position);

        public Vector2D Velocity = new Vector2D();

        internal Vector2D Position
        {
            get => Owner.Components.Get<Spatial>().Position;
            set => Owner.Components.Get<Spatial>().Position = value;
        }
        public Vector2D PreviousPosition { get; internal set; }
        public Vector2D PreviousVelocity { get; internal set; }

        public RectangleBody(CollisionBox bounds, float mass, bool immovable)
        {
            this.Bounds = bounds;
            this.Mass = mass;
            this.Immovable = immovable;
        }
    }
}
