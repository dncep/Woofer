using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;

using WooferGame.Systems.Camera;
using WooferGame.Systems.Camera.Shake;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Environment;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;
using WooferGame.Systems.Player.Animation;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Visual;
using WooferGame.Test_Data;

namespace WooferGame.Scenes
{
    class IntroScene : Scene
    {

        public IntroScene()
        {
            Entities.Add(new Room0(0, 128));
            //Entities.Add(new Room0(320, -32));

            //CurrentViewport.Scale = 2;

            Entities.Add(new PlayerEntity(96, 200));

            Entities.Add(new Checkpoint(96, 200, new Rectangle(-8, -8, 16, 16), true));
            Entities.Add(new Checkpoint(224, 296, new Rectangle(-8, -8, 16, 16)));
            Entities.Add(new Checkpoint(24, 360, new Rectangle(-8, -8, 16, 16)));

            Entities.Add(new DeathBarrier(-1000));

            Entities.Add(new BreakableGlassEntity(new Rectangle(54, 224 + 128, 8, 48)));

            //Input
            Systems.Add(new PlayerMovement());
            Systems.Add(new PlayerOrientationSystem());

            //Tick
            Systems.Add(new PhysicsSystem());
            Systems.Add(new PulseSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new CameraShakeSystem());
            Systems.Add(new CheckpointSystem());
            Systems.Add(new DeathBarrierSystem());

            //Systems.Add(new DebugSystem());

            Systems.Add(new GlassBreakingSystem());

            //Rendering
            Systems.Add(new PlayerAnimationSystem());
            Systems.Add(new LevelRenderer());
        }
    }

    internal class Room0 : LevelSection
    {
        public Room0(int x, int y) : base("room0", new Rectangle(x, y, 432, 400))
        {
            this.AddCollision(new CollisionBox(0, 0, 314, 64)); //Floor
            this.AddCollision(new CollisionBox(0, 64, 64, 160)); //Left Wall
            this.AddCollision(new CollisionBox(32, 272, 32, 96)); //Left Wall Above Glass
            //this.AddCollision(new CollisionBox(54, 224, 8, 48)); //Glass
            this.AddCollision(new CollisionBox(288, 112, 32, 256)); //Right Wall
            this.AddCollision(new CollisionBox(208, 152, 80, 8)
            {
                RightFaceProperties = new CollisionFaceProperties(),
                BottomFaceProperties = new CollisionFaceProperties(),
                LeftFaceProperties = new CollisionFaceProperties()
            }); //Shelf
            this.AddSlope(new Vector2D(374, 32), new Vector2D(314, 64), 0.5);
            this.AddCollision(new CollisionBox(314, 0, 118, 32)); //Floor under slope
            this.AddCollision(new CollisionBox(0, 368, 432, 32)); //Ceiling
            this.FinalizeCollision();
        }
    }
}
