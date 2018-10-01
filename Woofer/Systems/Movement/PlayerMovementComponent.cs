using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor;

namespace WooferGame.Systems.Movement
{
    [Component("player_controlled")]
    class PlayerMovementComponent : Component
    {
        [PersistentProperty]
        public float WalkSpeed { get; set; } = 100;
        [PersistentProperty]
        public float MaxWalkSpeed { get; set; } = 100;
        [PersistentProperty]
        public float AirborneSpeed { get; set; } = 6;
        [PersistentProperty]
        public float MaxAirborneSpeed { get; set; } = 30;
        [PersistentProperty]
        public float JumpSpeed { get; set; } = 192;

        public double CurrentSpeed => OnGround ? WalkSpeed : AirborneSpeed;
        public double CurrentMaxSpeed => OnGround ? MaxWalkSpeed : MaxAirborneSpeed;

        public bool OnGround { get; internal set; } = false;
    }
}