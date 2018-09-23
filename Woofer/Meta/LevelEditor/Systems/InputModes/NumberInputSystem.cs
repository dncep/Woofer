using System;
using System.Collections.Generic;
using System.Drawing;
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
using WooferGame.Common;
using Rectangle = EntityComponentSystem.Util.Rectangle;

namespace WooferGame.Meta.LevelEditor.Systems.InputModes
{
    [ComponentSystem("NumberInputSystem", ProcessingCycles.Input | ProcessingCycles.Render, ProcessingFlags.Pause)]
    class NumberInputSystem : ComponentSystem
    {
        private List<GUIButton> Pad;

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            
        }
    }

    internal class GUIButton
    {
        public Vector2D Position;
        public string Display;
        public Rectangle Bounds;
        public bool Enabled = true;
        public bool Highlighted = false;

        public GUIButton(Vector2D position, string display, Rectangle bounds)
        {
            Position = position;
            Display = display;
            Bounds = bounds;
        }

        public void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r, DirectGraphicsContext<TSurface, TSource> layer, Vector2D offset)
        {
            layer.FillRect((Bounds + (Position + offset)).ToDrawing(), Color.FromArgb(37, 37, 38));
            TextUnit label = new TextUnit(Display);
            Rectangle displayBounds = (Bounds + (Position + offset));
            System.Drawing.Size labelSize = label.GetSize(3);
            label.Render(r, layer, new Point((int)(displayBounds.X - displayBounds.Width / 2 - labelSize.Width / 2), (int)(displayBounds.Y - displayBounds.Height / 2 - labelSize.Height / 2)), 3);
        }
    }
}
