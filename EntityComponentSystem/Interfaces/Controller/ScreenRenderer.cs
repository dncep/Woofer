using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GameInterfaces.GraphicsInterface;

namespace GameInterfaces.Controller
{
    public class ScreenRenderer<TSurface, TSource>
    {
        public readonly IGraphicsContext<TSurface, TSource> GraphicsContext;

        private IRenderingUnit renderingUnit;
        private Dictionary<string, TSurface> layers = new Dictionary<string, TSurface>();

        public ISpriteManager<TSource> SpriteManager;

        public ScreenRenderer(IGraphicsContext<TSurface, TSource> graphicsContext, ISpriteManager<TSource> spriteManager, IRenderingUnit renderingUnit)
        {
            this.GraphicsContext = graphicsContext;
            this.SpriteManager = spriteManager;
            UpdateLayers(renderingUnit);
        }

        public void ContextCompletion(object sender, TSurface surface)
        {
            /*string layerName = layers.Where(p => ReferenceEquals(surface, p.Value)).First().Key;
            IRenderingLayer layer = renderingUnit.Layers[layerName];*/
        }

        internal void UpdateLayers(IRenderingUnit renderingUnit)
        {
            this.renderingUnit = renderingUnit;

            foreach(KeyValuePair<string, TSurface> pair in layers)
            {
                GraphicsContext.DisposeSurface(pair.Value);
            }
            layers.Clear();

            foreach(IRenderingLayer layer in renderingUnit.Layers.Values)
            {
                layers.Add(layer.Name, GraphicsContext.CreateTarget(layer.LayerSize.Width, layer.LayerSize.Height));
            }

            /*ScreenResolution.Width = LockedResolutionWidth;
            ScreenResolution.Height = (int)(newSize.Height / ((float)newSize.Width / LockedResolutionWidth));

            if (View != null) graphicsContext.DisposeSurface(View);
            View = graphicsContext.CreateTarget(ScreenResolution.Width + 1, ScreenResolution.Height + 1);*/
        }

        public DirectGraphicsContext<TSurface, TSource> GetLayerGraphics(string name)
        {
            return new DirectGraphicsContext<TSurface, TSource>(layers[name], GraphicsContext, ContextCompletion);
        }

        public void Clear()
        {
            foreach(TSurface surface in layers.Values)
            {
                GraphicsContext.Clear(surface, Color.Transparent);
            }
        }

        public void Update()
        {
            foreach(KeyValuePair<string, TSurface> pair in layers)
            {
                GraphicsContext.Update(pair.Value, renderingUnit.Layers[pair.Key].Destination);
            }

            GraphicsContext.Reset();
        }
    }
}
