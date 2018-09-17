using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Player
{
    [Component("player_orientation")]
    class PlayerOrientation : Component
    {
        public Vector2D OriginOffset => new Vector2D(0, 16);
        
        [PersistentProperty]
        public Vector2D Unit { get; set; }

        public double Angle => Unit.Angle;
    }
}
