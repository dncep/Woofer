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

        void Draw(TSurface subject, TSurface target, Rectangle destination, Rectangle? source = null, DrawInfo info = default(DrawInfo));
        void Draw(TSource subject, TSurface target, Rectangle destination, Rectangle? source = null, DrawInfo info = default(DrawInfo));

        void FillRect(TSurface target, Rectangle rectangle, Color color);

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

    public struct DrawInfo
    {
        public DrawMode Mode;
        public Color? Color;
    }
}
