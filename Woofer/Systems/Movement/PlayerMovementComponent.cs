using EntityComponentSystem.Components;

using WooferGame.Input;

namespace WooferGame.Systems.Movement
{
    [Component("player_controlled")]
    class PlayerMovementComponent : Component
    {
        public double WalkSpeed { get; } = 100;
        public double MaxWalkSpeed { get; } = 100;
        public double AirborneSpeed { get; } = 20;
        public double MaxAirborneSpeed { get; } = 20;
        public double JumpSpeed { get; } = 192;

        public InputTimeframe Jump = new InputTimeframe(5);

        public double CurrentSpeed => OnGround ? WalkSpeed : AirborneSpeed;
        public double CurrentMaxSpeed => OnGround ? MaxWalkSpeed : MaxAirborneSpeed;

        public bool OnGround { get; internal set; } = false;
    }
}