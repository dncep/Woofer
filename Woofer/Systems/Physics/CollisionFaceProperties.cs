namespace WooferGame.Systems.Physics
{
    public struct CollisionFaceProperties
    {
        public bool Enabled;
        public float Friction;
        public bool Snap;

        public CollisionFaceProperties(bool enabled, float friction)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = false;
        }

        public CollisionFaceProperties(bool enabled, float friction, bool snap)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = snap;
        }

        public override string ToString() => $"[Enabled: {Enabled}, Friction: {Friction}]";
    }
}
