using System.Collections.Generic;
using System.Drawing;

using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;

namespace WooferGame.Controller
{
    class WooferRenderingUnit : IRenderingUnit
    {
        private readonly WooferController controller;
        public Size ScreenSize { get; private set; } = new Size(1280, 720);
        public Dictionary<string, IRenderingLayer> Layers { get; private set; } = new Dictionary<string, IRenderingLayer>();

        public WooferRenderingUnit(WooferController controller)
        {
            this.controller = controller;
            Layers["background"] = new BackgroundRenderingLayer(controller);
            Layers["level"] = new LevelRenderingLayer(controller);
        }

        public void LoadContent<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer)
        {
            ISpriteManager<TSource> SpriteManager = screenRenderer.SpriteManager;
            SpriteManager.LoadSprite("notes");
            SpriteManager.LoadSprite("sprites0");
            SpriteManager.LoadSprite("intro_bg");

            ClipSprite("brick", "sprites0", new Rectangle(16, 0, 16, 16), screenRenderer.GraphicsContext, screenRenderer.SpriteManager);
            ClipSprite("grass", "sprites0", new Rectangle(0, 0, 16, 16), screenRenderer.GraphicsContext, screenRenderer.SpriteManager);

            ClipSprite("brick_slope_left", "sprites0", new Rectangle(16, 16, 16, 16), screenRenderer.GraphicsContext, screenRenderer.SpriteManager);
            ClipSprite("brick_slope_right", "sprites0", new Rectangle(16, 32, 16, 16), screenRenderer.GraphicsContext, screenRenderer.SpriteManager);

            ClipSprite("room0", "intro_bg", new Rectangle(0, 0, 432, 400), screenRenderer.GraphicsContext, screenRenderer.SpriteManager);
        }

        private void ClipSprite<TSurface, TSource>(string spriteName, string spritesheet, Rectangle spriteBounds, IGraphicsContext<TSurface, TSource> graphicsContext, ISpriteManager<TSource> spriteManager)
        {
            TSurface target = graphicsContext.CreateTarget(spriteBounds.Width, spriteBounds.Height);
            graphicsContext.Draw(spriteManager[spritesheet], target, new Rectangle(0, 0, spriteBounds.Width, spriteBounds.Height), spriteBounds);
            spriteManager[spriteName] = graphicsContext.TargetToSource(target);
        }

        public void Draw<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer) {
            controller.ActiveScene.Draw(screenRenderer);
        }
    }
}
