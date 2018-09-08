using EntityComponentSystem.Components;

namespace WooferGame.Systems.DeathBarrier
{
    [Component("death_barrier")]
    class DeathBarrierComponent : Component
    {
        public double Y { get; private set; }

        public DeathBarrierComponent(double y) => Y = y;
    }
}
