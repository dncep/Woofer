using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;

using WooferGame.Systems.Camera;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Player.Animation;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Player
{
    class PlayerEntity : Entity
    {
        public PlayerEntity(double x, double y)
        {
            //Physics
            Components.Add(new Spatial(x, y));
            Components.Add(new Physical());
            Components.Add(new SoftBody(new CollisionBox(-6, 0, 12, 16), 4f));
            Components.Add(new PulsePushable());

            //Rendering
            Components.Add(new Renderable());
            Components.Add(new LevelRenderable());

            Components.Add(new PlayerAnimation("char"));

            //Movement
            Components.Add(new PlayerMovementComponent());
            Components.Add(new PlayerOrientation());
            Components.Add(new PulseAbility());
            
            //Camera
            Components.Add(new CameraTracked() { Offset = new Vector2D(0, 24) });

            //Checkpoints
            Components.Add(new CheckpointTrigger());
            Components.Add(new CheckpointOnBarrierComponent());
        }
    }
}
