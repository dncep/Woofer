using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;

using WooferGame.Scenes.CommonSprites;
using WooferGame.Scenes.LevelObjects;
using WooferGame.Systems.Camera;
using WooferGame.Systems.Camera.Shake;
using WooferGame.Systems.Checkpoints;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Debug;
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

namespace WooferGame.Scenes
{
    class IntroScene : Scene
    {

        public IntroScene()
        {
            Entities.Add(new Room3(78 * 16, 128));
            Entities.Add(new Room2(62 * 16, 128));
            Entities.Add(new Room1(640, 128));
            Entities.Add(new Room0(0, 128));

            //CurrentViewport.Scale = 2;

            Entities.Add(new Checkpoint(96, 264, new Rectangle(-8, -8, 16, 16), true));
            Entities.Add(new Checkpoint(224, 360, new Rectangle(-8, -8, 16, 16)));
            Entities.Add(new Checkpoint(24, 424, new Rectangle(-8, -8, 16, 16)));

            Entities.Add(new DeathBarrier(0));

            //Entities.Add(new EnergyRefillOrb(new Vector2D(440, 360)));

            //Entities.Add(new Sailboat(new Vector2D(600, 180)));

            Entities.Add(new PlayerEntity(256, 264));

            //Input
            Systems.Add(new PlayerMovement());
            Systems.Add(new PlayerOrientationSystem());
            Systems.Add(new InteractionSystem());

            //Tick
            Systems.Add(new SwitchSystem());
            Systems.Add(new PhysicsSystem());
            Systems.Add(new ActivationSystem());
            Systems.Add(new PulseSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new CameraShakeSystem());
            Systems.Add(new CheckpointSystem());
            Systems.Add(new DeathBarrierSystem());
            Systems.Add(new GlassBreakingSystem());
            Systems.Add(new TimerSystem());
            Systems.Add(new DoorSystem());

            //Other
            Systems.Add(new PlayerFeedbackSystem());
            Systems.Add(new FollowingSystem());
            Systems.Add(new EnergyRefillSystem());
            Systems.Add(new SailboatSystem());
            Systems.Add(new CornerAvoidanceSystem());
            Systems.Add(new CameraRegionSystem());

            //Systems.Add(new DebugSystem());
            
            //Rendering
            Systems.Add(new PlayerAnimationSystem());
            Systems.Add(new AnimationSystem());
            Systems.Add(new LevelRenderer());
            Systems.Add(new ParticleSystem());
            Systems.Add(new InteractionIconSystem());
            Entities.Add(new InteractionIconEntity());

            //DEBUG
            Systems.Add(new DebugClipping());

        }
    }

    internal class Room0 : LevelSection
    {
        public Room0(int x, int y) : base("room0", new Vector2D(x, y), new Rectangle(0, 64, 432, 400))
        {

            Renderable renderable = this.Components.Get<Renderable>();

            renderable.Sprites.Add(new Vent(new Vector2D(5 * 16, 22 * 16), HorizontalDirection.Left));
            renderable.Sprites.Add(new Vent(new Vector2D(15 * 16, 22 * 16), HorizontalDirection.Right));

            LabRoomBuilder rb = new LabRoomBuilder(40, 43, "lab_tileset", 7);
            rb.Fill(new Rectangle(0, 0, 20, 8), true); //Floor
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(0, 0, 19.75, 8)
            {
                RightFaceProperties = new CollisionFaceProperties()
            }); //Floor

            this.AddSegment(rb, new Rectangle(0, 8, 4, 10)); //Left Wall
            this.AddSegment(rb, new Rectangle(2, 21, 2, 6)); //Left Wall Above Glass
            this.AddSegment(rb, new Rectangle(18, 11, 2, 16)); //Right Wall

            rb.Fill(new Rectangle(20, 0, 20, 6), true); //Floor after slope
            this.AddCollision(CoordinateMode.Grid, new CollisionBox(20, 0, 20, 6)
            {
                TopFaceProperties = new CollisionFaceProperties(true, 0.3, true)
            }); //Floor after slope

            this.AddSegment(rb, new Rectangle(0, 27, 40, 10)); //Ceiling

            this.FinalizeCollision();
            rb.ResolveNeighbors();

            rb.Fill(new Rectangle(18, 4, 7, 2), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Fill(new Rectangle(20, 6, 2, 1), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Fill(new Rectangle(18, 6, 2, 1), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b00110111, TileMapOffset = new Vector2D(0, 256) });
            rb.Set(19, 7, new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b00110111 });

            rb.Fill(new Rectangle(35, 4, 5, 2), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Fill(new Rectangle(37, 5, 2, 1), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Fill(new Rectangle(38, 6, 2, 1), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111111, TileMapOffset = new Vector2D(0, 256) });

            renderable.Sprites.AddRange(rb.Build());

            this.QueueEntity(new BreakableGlassEntity(new Rectangle(3.375 * 16, 18 * 16, 0.5 * 16, 3 * 16), new Rectangle(0, 160, 40, 48), new Vector2D(-2 * 16, 0)));

            this.QueueEntity(new Shelf(new Vector2D(18 * 16, 13.5 * 16), HorizontalDirection.Left));

            this.QueueEntity(new Ramp(new Vector2D(24 * 16, 6 * 16), new Vector2D(20 * 16, 8 * 16), 0.5, new Vector2D(8, 0)));
            this.QueueEntity(new Ramp(new Vector2D(36 * 16, 6 * 16), new Vector2D(40 * 16, 8 * 16), 0.5, new Vector2D(-8, 0)));

            Rectangle cameraArea = new Rectangle(3 * 16, 15 * 16, 17 * 16, 10 * 16);

            this.QueueEntity(new CameraRegion(cameraArea, cameraArea.Center + new Vector2D(0, -32)));
        }
    }

    internal class Room1 : LevelSection
    {
        public Room1(int x, int y) : base("room0", new Vector2D(x, y), new Rectangle(0, 64, 0, 0))
        {
            Renderable renderable = this.Components.Get<Renderable>();

            LabRoomBuilder rb = new LabRoomBuilder(22, 43, "lab_tileset");
            this.AddSegment(rb, new Rectangle(0, 0, 22, 8)); //Floor
            this.AddSegment(rb, new Rectangle(4, 11, 2, 5)); //Left Wall
            this.AddSegment(rb, new Rectangle(20, 11, 2, 5)); //Right Wall
            this.AddSegment(rb, new Rectangle(4, 15, 18, 22)); //Ceiling
            this.AddSegment(rb, new Rectangle(0, 27, 4, 10)); //Previous room extended ceiling
            rb.Set(13, 7, false);
            this.QueueEntity(new MovableBox(new Vector2D(17 * 16, 9 * 16)));
            PulseEmitter emitter = new PulseEmitter(new Vector2D(13.5 * 16, 7.5 * 16), Vector2D.UnitJ, 128, 48);
            this.QueueEntity(emitter);

            this.QueueEntity(new InteractableButton(new Vector2D(7.5 * 16, 9.5 * 16), emitter.Id));

            Door door = new Door(new Vector2D(21 * 16, 11 * 16), true);
            this.QueueEntity(door);

            Switch @switch = new Switch(new Vector2D(13.5 * 16, 15 * 16), new Rectangle(-16, -4, 32, 4));
            @switch.Components.Add(new LinkedActivationComponent(door.Id));
            this.QueueEntity(@switch);

            Rectangle cameraArea = new Rectangle(4 * 16, 8 * 16, 18 * 16, 8 * 16);

            this.QueueEntity(new CameraRegion(cameraArea, cameraArea.Center + new Vector2D(0, -16)));

            this.QueueEntity(new Checkpoint(5.5 * 16, 8 * 16, new Rectangle(-24, 0, 32, 48)));

            rb.ResolveNeighbors();
            renderable.Sprites.AddRange(rb.Build());
            this.FinalizeCollision();
        }
    }

    internal class Room2 : LevelSection
    {
        public Room2(int x, int y) : base("room0", new Vector2D(x, y), new Rectangle(0, 64, 0, 0))
        {
            Renderable renderable = this.Components.Get<Renderable>();

            LabRoomBuilder rb = new LabRoomBuilder(16, 43, "lab_tileset");
            this.AddSegment(rb, new Rectangle(0, 0, 16, 8)); //Floor
            this.AddSegment(rb, new Rectangle(0, 11, 2, 9)); //Left Wall
            this.AddSegment(rb, new Rectangle(13, 16, 2, 6)); //Right Wall Upper
            this.AddSegment(rb, new Rectangle(13, 8, 3, 5)); //Right Wall Lower
            this.AddSegment(rb, new Rectangle(0, 19, 16, 18)); //Ceiling
            this.AddSegment(rb, new Rectangle(2, 11, 3, 2));
            this.AddSegment(rb, new Rectangle(10, 11, 3, 2));

            rb.Set(7, 7, false);
            PulseEmitter emitter = new PulseEmitter(new Vector2D(7.5 * 16, 7.5 * 16), Vector2D.UnitJ, 128, 48);
            emitter.Components.Add(new TimerComponent(1));
            this.QueueEntity(emitter);

            Door door = new Door(new Vector2D(14 * 16, 16 * 16), false);
            this.QueueEntity(door);

            this.QueueEntity(new InteractableButton(new Vector2D(3.5 * 16, 14.5 * 16), door.Id));

            Rectangle cameraArea = new Rectangle(0 * 16, 9 * 16, 15 * 16, 10 * 16);

            this.QueueEntity(new CameraRegion(cameraArea, cameraArea.Center + new Vector2D(8, -32)));

            this.QueueEntity(new Checkpoint(5.5 * 16, 8 * 16, new Rectangle(-24, 0, 32, 48)));

            rb.ResolveNeighbors();

            rb.Fill(new Rectangle(15, 3, 1, 8), new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b11111011, TileMapOffset = new Vector2D(0, 256) });
            rb.Set(15, 2, new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b10111111, TileMapOffset = new Vector2D(0, 256) });
            rb.Set(15, 11, new RoomTileRaw() { Enabled = true, Initialized = true, Neighbors = 0b00010011, TileMapOffset = new Vector2D(0, 256) });

            renderable.Sprites.AddRange(rb.Build());
            this.FinalizeCollision();
        }
    }

    internal class Room3 : LevelSection
    {
        public Room3(int x, int y) : base("room0", new Vector2D(x, y), new Rectangle(0, 64, 0, 0))
        {
            Renderable renderable = this.Components.Get<Renderable>();

            LabRoomBuilder rb = new LabRoomBuilder(16, 64, "lab_tileset");
            this.AddSegment(rb, new Rectangle(0, 0, 16, 5)); //Floor
            this.AddSegment(rb, new Rectangle(0, 5, 1, 8)); //Left Wall
            this.AddSegment(rb, new Rectangle(13, 16, 3, 6)); //Right Wall Upper
            this.AddSegment(rb, new Rectangle(13, 5, 3, 8)); //Right Wall Lower
            this.AddSegment(rb, new Rectangle(0, 19, 16, 18)); //Ceiling

            rb.Set(1, 4, false);
            PulseEmitter emitter = new PulseEmitter(new Vector2D(1.5 * 16, 4.5 * 16), Vector2D.UnitJ, 152, 48);
            emitter.Components.Add(new TimerComponent(1));
            this.QueueEntity(emitter);

            this.QueueEntity(new Shelf(new Vector2D(1 * 16, 12 * 16), HorizontalDirection.Right));

            PulseEmitter sideEmitter = new PulseEmitter(new Vector2D(3 * 16, 14 * 16), Vector2D.UnitI, 192, 48, solid: false);
            this.QueueEntity(sideEmitter);
            
            Door door = new Door(new Vector2D(15 * 16, 16 * 16), false);
            this.QueueEntity(door);

            this.QueueEntity(new InteractableButton(new Vector2D(13.5 * 16, 13.5 * 16), door.Id));

            this.QueueEntity(new InteractableButton(new Vector2D(4.5 * 16, 13.5 * 16), sideEmitter.Id));

            Rectangle cameraArea = new Rectangle(0 * 16, 9 * 16, 15 * 16, 10 * 16);

            this.QueueEntity(new CameraRegion(cameraArea, cameraArea.Center + new Vector2D(8, -16)));

            this.QueueEntity(new Checkpoint(5.5 * 16, 8 * 16, new Rectangle(-24, 0, 32, 48)));

            rb.ResolveNeighbors();
            renderable.Sprites.AddRange(rb.Build());
            this.FinalizeCollision();
        }
    }
}
