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
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Linking;
using WooferGame.Systems.Movement;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Player;
using WooferGame.Systems.Player.Animation;
using WooferGame.Systems.Player.Feedback;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Puzzles;
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
            Entities.Add(new Room1(640, 128));
            Entities.Add(new Room0(0, 128));

            //CurrentViewport.Scale = 2;

            Entities.Add(new Checkpoint(96, 200, new Rectangle(-8, -8, 16, 16), true));
            Entities.Add(new Checkpoint(224, 296, new Rectangle(-8, -8, 16, 16)));
            Entities.Add(new Checkpoint(24, 360, new Rectangle(-8, -8, 16, 16)));

            Entities.Add(new DeathBarrier(-1000));

            Entities.Add(new EnergyRefillOrb(new Vector2D(440, 296)));

            //Entities.Add(new Sailboat(new Vector2D(600, 180)));

            Entities.Add(new PulseEmitter(new Vector2D(440, 168), Vector2D.UnitJ, 128, 48));

            Entities.Add(new PlayerEntity(96, 200));

            //Input
            Systems.Add(new PlayerMovement());
            Systems.Add(new PlayerOrientationSystem());
            Systems.Add(new InteractionSystem());

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
            Systems.Add(new CornerAvoidanceSystem());

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

            Renderable renderable = this.Components.Get<Renderable>();

            renderable.Sprites.Add(new Vent(new Vector2D(80, 288), HorizontalDirection.Left));
            renderable.Sprites.Add(new Vent(new Vector2D(240, 288), HorizontalDirection.Right));

            LabRoomBuilder rb = new LabRoomBuilder(36, 400 / 16, "lab_tileset");
            rb.Fill(new Rectangle(0, 0, 20, 4), true); //Floor
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(0, 0, 19.75, 4)
            {
                RightFaceProperties = new CollisionFaceProperties()
            }); //Floor

            rb.Fill(new Rectangle(0, 4, 4, 10), true); //Left Wall
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(0, 4, 4, 10)); //Left Wall

            rb.Fill(new Rectangle(2, 17, 2, 6), true); //Left Wall Above Glass
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(2, 17, 2, 6)); //Left Wall Above Glass

            rb.Fill(new Rectangle(18, 7, 2, 16), true); //Right Wall
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(18, 7, 2, 16)); //Right Wall

            rb.Fill(new Rectangle(20, 0, 16, 2), true); //Floor after slope
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(19.75, 0, 17.25, 2)
            {
                TopFaceProperties = new CollisionFaceProperties(true, 0.3, true)
            }); //Floor after slope
            rb.Fill(new Rectangle(0, 23, 27, 2), true); //Ceiling
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(0, 23, 27, 2)); //Ceiling

            this.FinalizeCollision();
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

    internal class Room1 : LevelSection
    {
        public Room1(int x, int y) : base("room0", new Rectangle(x, y, 0, 0))
        {
            Renderable renderable = this.Components.Get<Renderable>();

            LabRoomBuilder rb = new LabRoomBuilder(64, 25, "lab_tileset");
            this.AddSegment(rb, new Rectangle(0, 0, 64, 4)); //Floor
            this.AddSegment(rb, new Rectangle(4, 7, 2, 5)); //Left Wall
            this.AddSegment(rb, new Rectangle(24, 7, 2, 5)); //Right Wall
            this.AddSegment(rb, new Rectangle(4, 11, 22, 6)); //Ceiling
            this.AddSegment(rb, new Rectangle(24, 4, 1, 8));
            rb.Set(13, 3, false);
            this.QueueEntity(new MovableBox(new Vector2D(13 * 16, 5 * 16)));
            this.QueueEntity(new PulseEmitter(new Vector2D(13.5 * 16, 3.5 * 16), Vector2D.UnitJ, 128, 48));

            rb.ResolveNeighbors();
            renderable.Sprites.AddRange(rb.Build());
            this.FinalizeCollision();
        }

        private void AddSegment(LabRoomBuilder rb, Rectangle rect)
        {
            rb.Fill(rect, true);
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(rect.X, rect.Y, rect.Width, rect.Height));
        }
    }
}
