namespace WooferGame.Systems.Physics
{
    public struct CollisionFaceProperties
    {
        public bool Enabled;
        public double Friction;
        public bool Snap;
        public double AttractDistance;

        public CollisionFaceProperties(bool enabled, double friction)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = false;
            AttractDistance = 0;
        }

        public CollisionFaceProperties(bool enabled, double friction, bool snap)
        {
            Enabled = enabled;
            Friction = friction;
            Snap = snap;
            AttractDistance = 0;
        }

        public override string ToString() => $"[Enabled: {Enabled}, Friction: {Friction}]";
    }
}
