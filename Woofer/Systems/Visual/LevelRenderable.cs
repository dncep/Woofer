using EntityComponentSystem.Components;

namespace WooferGame.Systems.Visual
{
    [Component("level_renderable")]
    class LevelRenderable : Component
    {
        public float ZOrder;

        public LevelRenderable() : this(0)
        {
        }

        public LevelRenderable(float zOrder) => ZOrder = zOrder;
    }
}
