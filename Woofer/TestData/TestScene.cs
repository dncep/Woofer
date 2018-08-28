using System.Threading;
using EntityComponentSystem.Components;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameBase;
using GameInterfaces.Input;
using Woofer.Systems.Physics;
using Woofer.Test_Data;

namespace Woofer.Scenes
{
    class TestScene : Scene
    {
        //TestEntity a;

        Box box;

        public TestScene() : base()
        {
            //Entities.Add(new TestEntity(0, 0));
            //Entities.Add(new TestEntity(10, 10));
            //Entities.Add(new TestEntity(137+10, 125));
            //CurrentViewport.X = 16;
            /*for(int i = 0; i < 100; i++)
            {
                Entities.Add(new TestEntity(0, i));
            }*/
            //CurrentViewport.Scale = 1f;
            CurrentViewport.Y = 64;

            //Entities.Add(new TestEntity(0, 0));


            /*SpriteSheet sprites0 = new SpriteSheet(Properties.Resources.sprites0);
            sprites0.AddRegion("grass", 0, 0, 16, 16);
            sprites0.AddRegion("brick", 16, 0, 16, 16);
            sprites0.AddRegion("brick_slope_right", 16, 16, 16, 16);
            sprites0.AddRegion("brick_slope_left", 16, 32, 16, 16);
            sprites0.AddRegion("brick_slope_upside_down", 16, 48, 16, 16);
            Sprites.AddSpriteSheet(sprites0);

            SpriteSheet note = new SpriteSheet(Properties.Resources.notes);
            note.AddRegion("note", 0, 0, 32, 32);
            Sprites.AddSpriteSheet(note);*/

            //Entities.Add(new TileEntity("grass", 16, 8));
            for (int x = -1; x <= 30; x++)
            {
                Entities.Add(new TileEntity("brick", x, 0));
                Entities.Add(new TileEntity("brick", x, 8));

                //if (x < 2) Entities.Add(new TileEntity("brick", x, 1));
            }
            //Entities.Add(new TileEntity("brick", 5, 1));

            box = new Box(-24f, 64f);
            
            (box.Components[RectangleBody.Identifier] as RectangleBody).Velocity += new Vector2D(80, 0);
            Entities.Add(box);

            //Entities.Add(new Box(0f, 16f));
            //Entities.Add(new Slope(0, 1, true));
            //Entities.Add(new Slope(1, 1, false));

            //Input
            Systems.Add(new CameraMove());

            //Tick
            Systems.Add(new PhysicsSystem());

            //Rendering
            Systems.Add(new LevelRenderer());
            //Systems.Add(new ObjectRenderer());
        }


        protected override void Tick()
        {
            /*System.Console.WriteLine($"DeltaTime: \t{DeltaTime}");
            System.Console.WriteLine($"FixedDeltaTime: {FixedDeltaTime}");
            System.Console.WriteLine();*/


            var boxrb = (box.Components[RectangleBody.Identifier] as RectangleBody);
            if (Game1.GameController.InputUnit.GamePads[0].Buttons.A.IsPressed()) boxrb.Velocity.Y = 128;
            //var boxrb = (box.Components["rigidbody"]) as RigidBody;
            if (boxrb.Position.Y < -1000)
            {
                boxrb.Position = new Vector2D(8, 64);
                boxrb.Velocity = new Vector2D();
            }
            //CurrentViewport.Y = (float) (16d*Math.Sin(2*time));
            //Console.WriteLine("Ticking");
            
            //CurrentViewport.Scale = 2;
            //CurrentViewport.Scale += (float)DeltaTime * 1.1f;
            //CurrentViewport.Scale += (float)DeltaTime * change;
            //if (CurrentViewport.Scale >= 2) change = -0.1f;
            //else if (CurrentViewport.Scale <= 0.1) change = 0.1f;

            //(a.Components["physical"] as Physical).X += 10 * DeltaTime;
        }
    }
}
