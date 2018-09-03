using System;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameBase;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using GameInterfaces.Input;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Pulse;

namespace WooferGame.Test_Data
{
    class Box : Entity
    {
        public Box(double x, double y)
        {
            Components.Add(new Spatial(x, y));
            Components.Add(new Renderable("grass", new Rectangle(-8, -8, 16, 16)));
            Components.Add(new Physical());
            Components.Add(new SoftBody(new CollisionBox(-8, -8, 16, 16), 4f));
            Components.Add(new PulsePushable());
            Components.Add(new LevelTile());
        }

        public override string ToString() => "Box{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }

    class TileEntity : Entity
    {
        public TileEntity(string texture, int tileX, int tileY)
        {
            Components.Add(new Spatial(16 * tileX, 16 * tileY));
            Components.Add(new Renderable(texture, new Rectangle(-8, -8, 16, 16)));
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(new CollisionBox[] {new CollisionBox(-8, -8, 16, 16)
            {
                TopFaceProperties = new CollisionFaceProperties(true, 0.3)/*,
                LeftFaceProperties = new CollisionFaceProperties(),
                BottomFaceProperties = new CollisionFaceProperties(),
                RightFaceProperties = new CollisionFaceProperties()*/
            }}));
            Components.Add(new LevelTile());
        }

        public override string ToString() => "TileEntity{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }

    class Slope : Entity
    {
        public Slope(string texture, double tileX, double tileY)
        {
            Components.Add(new Spatial(16 * tileX, 16 * tileY));
            Components.Add(new Renderable(texture, new Rectangle(-8, -8, 16, 16)));
            Components.Add(new Physical() { GravityMultiplier = 0 });
            Components.Add(new RigidBody(new CollisionBox[] {
                new CollisionBox(-8, -8, 16, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(-6, -6, 14, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(-4, -4, 12, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(-2, -2, 10, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(0, 0, 8, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(2, 2, 6, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(4, 4, 4, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                },
                new CollisionBox(6, 6, 2, 2)
                {
                    TopFaceProperties = new CollisionFaceProperties(true, 0.3, true),
                    LeftFaceProperties = new CollisionFaceProperties(),
                    RightFaceProperties = new CollisionFaceProperties()
                }
            }));
            Components.Add(new LevelTile());
        }

        public override string ToString() => "Slab{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }

    [Component("level_tile")]
    class LevelTile : Component
    {
        public string Texture { get; private set; }
    }

    [Component("renderable")]
    class Renderable : Component
    {
        public string Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Renderable(string texture)
        {
            this.Texture = texture;
        }

        public Renderable(string texture, Rectangle bounds)
        {
            this.Texture = texture;
            this.Bounds = bounds;
        }

        public void Render<TSurface, TSource>(DirectGraphicsContext<TSurface, TSource> layer, CameraView view, ScreenRenderer<TSurface, TSource> r)
        {
            IGameController controller = Woofer.Controller;

            Spatial spatial = Owner.Components["spatial"] as Spatial;
            float x = ((float)(spatial.X - 8));
            float y = ((float)(-spatial.Y - 8));
            int size = 16;

            x -= (int)controller.ActiveScene.CurrentViewport.X;
            y += (int)controller.ActiveScene.CurrentViewport.Y;
            
            x += layer.GetSize().Width / 2;
            y += layer.GetSize().Height / 2;

            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), size, size);
            
            layer.Draw(r.SpriteManager[Texture], drawingRect);
        }
    }

    [ComponentSystem("fpscounter")]
    public class FramerateCounter : ComponentSystem
    {
        private float _elapsed = 0;
        private int frames = 0;
        public int Framerate { get; private set; }

        //private Font font;

        public FramerateCounter()
        {
            TickProcessing = true;
            RenderProcessing = true;

            //font = new Font(Game_Implementation.Properties.Resources.consola, 11);
        }

        public override void Tick()
        {
            frames++;
            _elapsed += Owner.DeltaTime;
            if (_elapsed >= 1)
            {
                _elapsed--;
                Framerate = frames;
                frames = 0;
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            /*r.BeginRenderingSection(false);

            Surface textSurface = font.Render($"{Framerate} fps", Color.Yellow);
            r.View.Blit(textSurface, new Point(0, 0));
            textSurface.Dispose();
            //g.DrawString(, new Font("Consolas", 12), new SolidBrush(Color.Yellow), 0, 0);

            r.EndRenderingSection();*/
        }
    }

    [ComponentSystem("level_renderer")]
    public class LevelRenderer : ComponentSystem
    {
        public LevelRenderer()
        {
            Watching = new string[] { "level_tile" };
            RenderProcessing = true;
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            /*var bg = r.GetLayerGraphics("background");
            bg.Clear(System.Drawing.Color.FromArgb(32, 255, 0, 255));
            bg.Complete();*/

            var layer = r.GetLayerGraphics("level");

            CameraView view = Owner.CurrentViewport;

            foreach (LevelTile tile in WatchedComponents)
            {
                Renderable renderable = tile.Owner.Components["renderable"] as Renderable;
                renderable.Render(layer, view, r);
            }

            layer.Complete();
        }
    }
}
