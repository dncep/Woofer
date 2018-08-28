using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInterfaces.GraphicsInterface
{
    public class DirectGraphicsContext<TSurface, TSource> : IDisposable
    {
        public readonly TSurface Surface;
        public readonly IGraphicsContext<TSurface, TSource> GraphicsContext;

        public event EventHandler<TSurface> CompletionEvent;

        public DirectGraphicsContext(TSurface surface, IGraphicsContext<TSurface, TSource> graphicsContext)
        {
            Surface = surface;
            GraphicsContext = graphicsContext;
        }

        public DirectGraphicsContext(TSurface surface, IGraphicsContext<TSurface, TSource> graphicsContext, EventHandler<TSurface> completionHandler)
        {
            Surface = surface;
            GraphicsContext = graphicsContext;
            CompletionEvent += completionHandler;
        }

        public void Clear(Color color) => GraphicsContext.Clear(Surface, color);
        public Size GetSize() => GraphicsContext.GetSize(Surface);
        public Size GetSize(TSource source) => GraphicsContext.GetSize(source);

        public void Draw(TSurface subject, Rectangle destination) => GraphicsContext.Draw(subject, Surface, destination);
        public void Draw(TSurface subject, Rectangle destination, Rectangle? source) => GraphicsContext.Draw(subject, Surface, destination, source);
        public void Draw(TSource subject, Rectangle destination) => GraphicsContext.Draw(subject, Surface, destination);
        public void Draw(TSource subject, Rectangle destination, Rectangle? source) => GraphicsContext.Draw(subject, Surface, destination, source);

        public TSurface CreateTarget(int width, int height) => GraphicsContext.CreateTarget(width, height);

        public TSource TargetToSource(TSurface target) => GraphicsContext.TargetToSource(target);

        public TSurface Scale(double scaleX, double scaleY, bool antialias) => GraphicsContext.Scale(Surface, scaleX, scaleY, antialias);
        public TSurface Scale(double scale, bool antialias) => GraphicsContext.Scale(Surface, scale, antialias);

        public void Update() => GraphicsContext.Update(Surface);
        public void Update(Rectangle destination) => GraphicsContext.Update(Surface, destination);
        public void DisposeSurface() => GraphicsContext.DisposeSurface(Surface);
        public void DisposeSource(TSource source) => GraphicsContext.DisposeSource(source);

        public void Reset() => GraphicsContext.Reset();

        public void Complete()
        {
            CompletionEvent.Invoke(this, Surface);
            Dispose();
        }

        public void Dispose() { }
    }
}
