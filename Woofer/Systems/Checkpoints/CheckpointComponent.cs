using EntityComponentSystem.Components;

namespace WooferGame.Systems.Checkpoints
{
    [Component("checkpoint")]
    class CheckpointComponent : Component
    {
        public bool Selected = false;
    }
}
