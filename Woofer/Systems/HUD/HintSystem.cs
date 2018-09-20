using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;
using GameInterfaces.GraphicsInterface;
using WooferGame.Systems.Interaction;

namespace WooferGame.Systems.HUD
{
    [ComponentSystem("HintSystem", ProcessingCycles.Tick | ProcessingCycles.Render),
        Listening(typeof(ShowTextEvent), typeof(ActivationEvent))]
    class HintSystem : ComponentSystem
    {
        private List<ShowTextEvent> Active = new List<ShowTextEvent>();

        private static readonly byte[] char_sizes =
        {
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            5, 4, 5, 7, 7, 8, 7, 5, 5, 5, 5, 7, 4, 7, 5, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 6, 6, 6, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 4, 7, 4, 7, 7,
            4, 6, 6, 6, 6, 6, 5, 6, 6, 3, 6, 5, 5, 7, 6, 7,
            6, 6, 6, 6, 5, 6, 7, 7, 7, 6, 6, 5, 5, 5, 8, 8
        };

        public override void EventFired(object sender, Event e)
        {
            if(e is ShowTextEvent te)
            {
                Active.Insert(0, te);
            }
            else if(e is ActivationEvent ae)
            {
                ShowTextComponent comp = ae.Affected.Components.Get<ShowTextComponent>();

                if (comp == null) return;

                Owner.Events.InvokeEvent(new ShowTextEvent(comp.Text, comp) { Duration = comp.Duration });
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if(Active.Count > 0)
            {
                var layer = r.GetLayerGraphics("hud");

                int destY = 3*layer.GetSize().Height/4;

                TSurface[] Rendered = new TSurface[Active.Count];

                for (int i = 0; i < Active.Count; i++)
                {
                    Rendered[i] = Active[i].Text.Render<TSurface, TSource>(r);
                }

                for(int i = 0; i < Active.Count; i++)
                {
                    ShowTextEvent current = Active[i];
                    TSurface surface = Rendered[i];
                    var surfaceOp = new DirectGraphicsContext<TSurface, TSource>(surface, r.GraphicsContext);

                    int width = surfaceOp.GetSize().Width;
                    int height = surfaceOp.GetSize().Height;
                    int destX = layer.GetSize().Width / 2 - width / 2;

                    var rect = new System.Drawing.Rectangle(destX, destY, width * current.TextSize, height * current.TextSize);
                    
                    layer.Draw(surface, rect);

                    destY -= height + 2;

                    surfaceOp.DisposeSurface();
                }
            }
        }
        public override void Tick()
        {
            for(int i = 0; i < Active.Count; i++)
            {
                ShowTextEvent te = Active[i];
                if (te.Duration > 0) te.Duration -= Owner.DeltaTime;
                if (te.Duration <= 0)
                {
                    te.Duration = 0;
                    Active.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
