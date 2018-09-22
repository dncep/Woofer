using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using WooferGame.Controller;
using WooferGame.Meta.LevelEditor.Systems.EntityOutlines;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Visual;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("entity_outliner", ProcessingCycles.Render),
        Listening(typeof(BeginEntityOutline), typeof(ClearEntityOutlines))]
    class EntityOutliner : ComponentSystem
    {
        private const int StrokeWidth = 2;
        private List<long> Highlighted = new List<long>();

        public override bool ShouldSave => false;

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var layer = r.GetLayerGraphics("hi_res_overlay");

            CameraView view = Owner.CurrentViewport;

            foreach (long entId in Highlighted)
            {
                if (entId == 0) continue;
                if (!Owner.Entities.ContainsId(entId)) continue;
                Entity entity = Owner.Entities[entId];
                Rectangle realBounds = EditorUtil.GetSelectionBounds(entity);
                if(realBounds != null)
                {
                    System.Drawing.Size screenSize = Woofer.Controller.RenderingUnit.ScreenSize;

                    float x = (float)realBounds.X;
                    float y = (float)realBounds.Y;
                    float width = (float)realBounds.Width;
                    float height = (float)realBounds.Height;

                    if (!new Rectangle(x, y, width, height)
                        .IntersectsWith(
                        new Rectangle(view.X - layer.GetSize().Width / 2, view.Y - layer.GetSize().Height / 2, layer.GetSize().Width, layer.GetSize().Height))) continue;

                    x -= (int)Math.Floor(view.X);
                    y -= (int)Math.Floor(view.Y);

                    y *= -1;
                    y -= height;

                    float scale = (screenSize.Width / (float)LevelRenderingLayer.LevelScreenSize.Width);

                    x *= scale;
                    y *= scale;
                    width *= scale;
                    height *= scale;

                    x += layer.GetSize().Width / 2;
                    y += layer.GetSize().Height / 2;

                    x = (float)Math.Floor(x);
                    y = (float)Math.Floor(y);
                    
                    x -= (int)(scale * GeneralUtil.EuclideanMod(view.X, 1));
                    y += (int)(scale * (GeneralUtil.EuclideanMod(view.Y, 1) - 1));

                    System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)width, (int)height);

                    var pixel = r.SpriteManager["pixel"];
                    layer.Draw(pixel, new System.Drawing.Rectangle(drawingRect.X, drawingRect.Y, drawingRect.Width, StrokeWidth));
                    layer.Draw(pixel, new System.Drawing.Rectangle(drawingRect.X, drawingRect.Y + drawingRect.Height - StrokeWidth, drawingRect.Width, StrokeWidth));
                    layer.Draw(pixel, new System.Drawing.Rectangle(drawingRect.X, drawingRect.Y, StrokeWidth, drawingRect.Height));
                    layer.Draw(pixel, new System.Drawing.Rectangle(drawingRect.X + drawingRect.Width - StrokeWidth, drawingRect.Y, StrokeWidth, drawingRect.Height));

                    //layer.Draw(r.SpriteManager["grass"], drawingRect);
                }
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is BeginEntityOutline begin)
            {
                Highlighted.Add(begin.Entity.Id);
            } else if(e is ClearEntityOutlines)
            {
                Highlighted.Clear();
            }
        }
    }
}
