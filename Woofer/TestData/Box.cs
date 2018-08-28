using System;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameBase;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using Woofer.Systems.Physics;

namespace Woofer.Test_Data
{
    class Box : Entity
    {
        public Box(float x, float y)
        {
            Components.Add(new Spatial(x, y));
            Components.Add(new Renderable("grass", new Rectangle(-8, -8, 16, 16)));
            Components.Add(new RectangleBody(new BoundingBox(-8, -8, 16, 16), 4f, false));
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
            Components.Add(new RectangleBody(new BoundingBox(-8, -8, 16, 16), 4f, true) { Velocity = (texture == "grass") ? new Vector2D(-16, 0) : new Vector2D()});
            Components.Add(new LevelTile());
        }

        public override string ToString() => "TileEntity{Texture=" + (Components["renderable"] as Renderable).Texture + ",Position=" + (Components["spatial"] as Spatial).Position + "}";
    }

    class LevelTile : Component
    {
        public string Texture { get; private set; }

        public LevelTile() => ComponentName = "level_tile";
    }

    class Renderable : Component
    {
        public string Texture { get; set; }
        public Rectangle Bounds { get; set; }

        public Renderable() => ComponentName = "renderable";

        public Renderable(string texture) : this()
        {
            this.Texture = texture;
        }

        public Renderable(string texture, Rectangle bounds) : this()
        {
            this.Texture = texture;
            this.Bounds = bounds;
        }

        public void Render<TSurface, TSource>(DirectGraphicsContext<TSurface, TSource> layer, CameraView view, ScreenRenderer<TSurface, TSource> r)
        {
            IGameController controller = Game1.GameController;

            Spatial physical = Owner.Components["spatial"] as Spatial;
            float x = ((float)physical.X - 8);
            float y = ((float)-physical.Y - 8);
            int size = 16;

            x -= (float) controller.ActiveScene.CurrentViewport.X;
            y += (float)controller.ActiveScene.CurrentViewport.Y;

            x += layer.GetSize().Width / 2;
            y += layer.GetSize().Height / 2;

            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), size, size);
            
            layer.Draw(r.SpriteManager[Texture], drawingRect);
        }
    }

    class CameraMove : ComponentSystem
    {
        public CameraMove()
        {
            this.SystemName = "free_camera";
            this.InputProcessing = true;
        }

        public override void Input()
        {
            Owner.CurrentViewport.Location += Game1.GameController.InputUnit.GamePads[0].Thumbsticks.Right;
        }
    }

    class Slope : Entity
    {
        public Slope(float x, float y, bool left)
        {
            Components.Add(new Spatial(16 * x, 16 * y));
            Components.Add(new Renderable(left ? "brick_slope_left" : "brick_slope_right", new Rectangle(-8, -8, 16, 16)));
            //Components.Add(new RigidBody(new Polygon(new Vector2D(8, -8), new Vector2D(-8, -8), new Vector2D(left ? 8 : -8, 8)), 4f, true));
            Components.Add(new LevelTile());
        }
    }

    public class FramerateCounter : ComponentSystem
    {
        private float _elapsed = 0;
        private int frames = 0;
        public int Framerate { get; private set; }

        //private Font font;

        public FramerateCounter()
        {
            SystemName = "fpscounter";
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

    public class LevelRenderer : ComponentSystem
    {
        public LevelRenderer()
        {
            SystemName = "level_renderer";
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
