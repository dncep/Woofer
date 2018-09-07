using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using WooferGame.Systems.Camera;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;
using WooferGame.Systems.Player.Actions;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes
{
    class IntroScene : Scene
    {

        public IntroScene()
        {
            Entities.Add(new Room0(0, 128));

            Entities.Add(new PlayerEntity(96, 208));

            Entities.Add(new Checkpoint(96, 200, true));

            Entities.Add(new DeathBarrier(-1000));

            //Input
            Systems.Add(new PlayerMovement());
            Systems.Add(new PlayerOrientationSystem());

            //Tick
            Systems.Add(new PhysicsSystem());
            Systems.Add(new PulseSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new CheckpointSystem());
            Systems.Add(new DeathBarrierSystem());

            //Rendering
            Systems.Add(new LevelRenderer());
        }
    }

    internal class Room0 : LevelSection
    {
        public Room0(int x, int y) : base("room0", new Rectangle(x, y, 352, 176))
        {
            this.AddCollision(new CollisionBox(0, 0, 304, 64)); //Floor
            this.AddCollision(new CollisionBox(0, 64, 64, 112)); //Left Wall
            this.AddCollision(new CollisionBox(288, 64, 64, 112)); //Right Wall
            this.AddCollision(new CollisionBox(208, 153, 80, 8)
            {
                RightFaceProperties = new CollisionFaceProperties(),
                BottomFaceProperties = new CollisionFaceProperties(),
                LeftFaceProperties = new CollisionFaceProperties()
            }); //Shelf
            this.FinalizeCollision();
        }
    }
}
