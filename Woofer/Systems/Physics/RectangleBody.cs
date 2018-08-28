using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace Woofer.Systems.Physics
{
    class RectangleBody : Component
    {
        public const string Identifier = "rectanglebody";

        public BoundingBox Bounds { get; private set; }
        public float Mass { get; set; }
        public float Friction { get; set; } = 0.3f;
        public bool Immovable { get; set; }

        //Friction:
        // Ice-like: 0.01f;
        // Brick-like: 0.3f

        public BoundingBox RealBounds => Bounds.Offset(Position);

        public Vector2D Velocity = new Vector2D();

        internal Vector2D Position
        {
            get => (Owner.Components[Spatial.Identifier] as Spatial).Position;
            set => (Owner.Components[Spatial.Identifier] as Spatial).Position = value;
        }
        public Vector2D PreviousPosition { get; internal set; }
        public Vector2D PreviousVelocity { get; internal set; }

        public RectangleBody(BoundingBox bounds, float mass, bool immovable)
        {
            ComponentName = Identifier;
            ListenedEvents = new string[] { "collision" };
            this.Bounds = bounds;
            this.Mass = mass;
            this.Immovable = immovable;
        }
    }
}
