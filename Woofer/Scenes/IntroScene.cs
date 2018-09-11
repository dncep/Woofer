using System;
using System.Linq;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using WooferGame.Scenes.CommonSprites;
using WooferGame.Scenes.LevelObjects;
using WooferGame.Systems.Camera;
using WooferGame.Systems.Camera.Shake;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Environment;
using WooferGame.Systems.Linking;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;
using WooferGame.Systems.Player.Animation;
using WooferGame.Systems.Player.Feedback;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Refill;
using WooferGame.Systems.Sailboat;
using WooferGame.Systems.Timer;
using WooferGame.Systems.Visual;
using WooferGame.Systems.Visual.Animation;
using WooferGame.Systems.Visual.Particles;
using WooferGame.Test_Data;

namespace WooferGame.Scenes
{
    class IntroScene : Scene
    {

        public IntroScene()
        {
            Entities.Add(new Room0(-80, 128));

            //CurrentViewport.Scale = 2;

            Entities.Add(new Checkpoint(96, 200, new Rectangle(-8, -8, 16, 16), true));
            Entities.Add(new Checkpoint(224, 296, new Rectangle(-8, -8, 16, 16)));
            Entities.Add(new Checkpoint(24, 360, new Rectangle(-8, -8, 16, 16)));

            Entities.Add(new DeathBarrier(-1000));

            Entities.Add(new EnergyRefillOrb(new Vector2D(360, 296)));

            Entities.Add(new Sailboat(new Vector2D(600, 180)));

            Entities.Add(new PulseEmitter(new Vector2D(360, 168), Vector2D.UnitJ, 128, 48));

            Entities.Add(new PlayerEntity(96, 200));


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
            Systems.Add(new GlassBreakingSystem());
            Systems.Add(new TimerSystem());

            //Other
            Systems.Add(new PlayerFeedbackSystem());
            Systems.Add(new FollowingSystem());
            Systems.Add(new EnergyRefillSystem());
            Systems.Add(new SailboatSystem());

            //Systems.Add(new DebugSystem());


            //Rendering
            Systems.Add(new PlayerAnimationSystem());
            Systems.Add(new AnimationSystem());
            Systems.Add(new LevelRenderer());
            Systems.Add(new ParticleSystem());
        }
    }

    internal class Room0 : LevelSection
    {
        public Room0(int x, int y) : base("room0", new Rectangle(x, y, 432, 400))
        {
            this.AddCollision(new CollisionBox(0, 0, 314, 64)
            {
                RightFaceProperties = new CollisionFaceProperties()
            }); //Floor
            this.AddCollision(new CollisionBox(0, 64, 64, 160)); //Left Wall
            this.AddCollision(new CollisionBox(32, 272, 32, 96)); //Left Wall Above Glass
            //this.AddCollision(new CollisionBox(54, 224, 8, 48)); //Glass
            this.AddCollision(new CollisionBox(288, 112, 32, 256)); //Right Wall
            /*this.AddCollision(new CollisionBox(208, 152, 80, 8)
            {
                RightFaceProperties = new CollisionFaceProperties(),
                BottomFaceProperties = new CollisionFaceProperties(),
                LeftFaceProperties = new CollisionFaceProperties()
            }); //Shelf*/
            //this.AddSlope(new Vector2D(374, 32), new Vector2D(314, 64), 0.5);
            this.AddCollision(new CollisionBox(314, 0, 278, 32)
            {
                TopFaceProperties = new CollisionFaceProperties(true, 0.3, true)
            }); //Floor after slope
            this.AddCollision(new CollisionBox(0, 368, 432, 32)); //Ceiling
            this.FinalizeCollision();

            Renderable renderable = this.Components.Get<Renderable>();

            renderable.Sprites.Add(new Vent(new Vector2D(80, 288), HorizontalDirection.Left));
            renderable.Sprites.Add(new Vent(new Vector2D(240, 288), HorizontalDirection.Right));


            LabRoomBuilder rb = new LabRoomBuilder(36, 400 / 16, "lab_tileset");
            rb.Fill(new Rectangle(0, 0, 20, 4), true);
            rb.Fill(new Rectangle(0, 4, 4, 10), true);
            rb.Fill(new Rectangle(2, 17, 2, 6), true);
            rb.Fill(new Rectangle(18, 7, 2, 16), true);
            rb.Fill(new Rectangle(20, 0, 16, 2), true);
            rb.Fill(new Rectangle(0, 23, 27, 2), true);

            rb.ResolveNeighbors();

            rb.Fill(new Rectangle(18, 0, 6, 2), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Set(20, 2, new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Fill(new Rectangle(18, 2, 2, 1), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b00110111, TileMapOffset = new Vector2D(0, 256) });
            rb.Set(19, 3, new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b00110111 });
            
            renderable.Sprites.AddRange(rb.Build());
            
            this.QueueEntity(new BreakableGlassEntity(new Rectangle(54, 224, 8, 48), new Rectangle(0, 160, 40, 48), new Vector2D(-32, 0)));

            this.QueueEntity(new Shelf(new Vector2D(288, 152), HorizontalDirection.Left));

            this.QueueEntity(new Ramp(new Vector2D(374, 32), new Vector2D(314, 64), 0.5, new Vector2D(6, 0)));
            this.QueueEntity(new Ramp(new Vector2D(386 + 192+4, 32), new Vector2D(446 + 192+4, 64), 0.5, new Vector2D(-6, 0)));

        }
    }
}
