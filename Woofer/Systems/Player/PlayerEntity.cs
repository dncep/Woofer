using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems.Camera;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Player
{
    class PlayerEntity : Entity
    {
        public PlayerEntity(double x, double y)
        {
            Components.Add(new Spatial(x, y));
            Components.Add(new Renderable("grass", new Rectangle(-8, -8, 16, 16)));
            Components.Add(new Physical());
            Components.Add(new SoftBody(new CollisionBox(-8, -8, 16, 16), 4f));
            Components.Add(new PulsePushable());
            Components.Add(new LevelRenderable());

            Components.Add(new PlayerMovementComponent());
            Components.Add(new CameraTracked() { Offset = new Vector2D(0, 24) });
            Components.Add(new PlayerOrientation());
            Components.Add(new PulseAbility());

            Components.Add(new CheckpointOnBarrierComponent());
        }
    }
}
