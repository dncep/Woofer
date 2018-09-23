using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Player
{
    [Component("player_orientation")]
    class PlayerOrientation : Component
    {
        [PersistentProperty]
        [Inspector(InspectorEditType.Offset)]
        public Vector2D OriginOffset => new Vector2D(0, 16);
        
        [PersistentProperty]
        public Vector2D Unit { get; set; }

        public double Angle => Unit.Angle;
    }
}
