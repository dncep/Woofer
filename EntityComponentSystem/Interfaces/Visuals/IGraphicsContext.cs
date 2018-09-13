using System.Drawing;
using EntityComponentSystem.Interfaces.Visuals;

namespace GameInterfaces.GraphicsInterface
{
    public interface IGraphicsContext<TSurface, TSource>
    {
        void Clear(TSurface surface, Color color);
        void Clear(Color color);
        Size GetSize(TSource source);
        Size GetSize(TSurface surface);

        Size GetScreenSize();

        void Draw(TSurface subject, TSurface target, Rectangle destination);
        void Draw(TSurface subject, TSurface target, Rectangle destination, Rectangle? source);
        void Draw(TSurface subject, TSurface target, Rectangle destination, DrawMode mode);
        void Draw(TSurface subject, TSurface target, Rectangle destination, Rectangle? source, DrawMode mode);
        void Draw(TSource subject, TSurface target, Rectangle destination);
        void Draw(TSource subject, TSurface target, Rectangle destination, Rectangle? source);
        void Draw(TSource subject, TSurface target, Rectangle destination, DrawMode mode);
        void Draw(TSource subject, TSurface target, Rectangle destination, Rectangle? source, DrawMode mode);
        TSurface CreateTarget(int width, int height);
        TSurface Scale(TSurface surface, double scaleX, double scaleY, bool antialias);
        TSurface Scale(TSurface surface, double scale, bool antialias);

        void Update(TSurface surface);
        void Update(TSurface surface, Rectangle destination);
        void DisposeSurface(TSurface surface);
        void DisposeSource(TSource source);
        TSource TargetToSource(TSurface target);
        void Reset();
    }
}
