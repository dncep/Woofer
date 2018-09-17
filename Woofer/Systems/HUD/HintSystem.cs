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

namespace WooferGame.Systems.HUD
{
    [ComponentSystem("HintSystem", ProcessingCycles.Tick | ProcessingCycles.Render),
        Listening(typeof(ShowTextEvent))]
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
                Active.Add(te);
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if(Active.Count > 0)
            {
                var layer = r.GetLayerGraphics("hud");
                var font = r.SpriteManager["font"];

                int destY = 3*layer.GetSize().Height/4;

                foreach(ShowTextEvent current in Active)
                {
                    int width = 0;

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(current.Text);

                    foreach (byte c in asciiBytes)
                    {
                        width += char_sizes[c] - 1;
                    }

                    width *= current.TextSize;

                    if(current.Icon != null)
                    {
                        width += (int)current.Icon.Destination.Width;
                        width += 4;
                    }

                    int destX = layer.GetSize().Width / 2 - width / 2;

                    if(current.Icon != null)
                    {
                        Rectangle iconDestination = new Rectangle(current.Icon.Destination);
                        iconDestination.X += destX;
                        iconDestination.Y += destY;
                        layer.Draw(r.SpriteManager[current.Icon.Texture], iconDestination.ToDrawing(), current.Icon.Source.ToDrawing());

                        destX += (int)iconDestination.Width + 4;
                    }

                    foreach (byte c in asciiBytes)
                    {
                        int srcX = (c % 16) * 8;
                        int srcY = (c / 16) * 8;

                        layer.Draw(font, new System.Drawing.Rectangle(destX, destY, 8* current.TextSize, 8* current.TextSize), new System.Drawing.Rectangle(srcX, srcY, 8, 8));
                        destX += (char_sizes[c] - 1) * current.TextSize;
                    }
                    destY -= Math.Max(8 * current.TextSize, current.Icon != null ? (int)current.Icon.Destination.Height : 0) + 2;
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
