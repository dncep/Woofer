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
using GameInterfaces.Input;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("EditorCursorSystem", ProcessingCycles.Input | ProcessingCycles.Tick | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(BeginModalChangeEvent))]
    class EditorCursorSystem : ComponentSystem
    {
        private Vector2D CursorPos;
        private bool InputActive = true;

        public override void Input()
        {
            if (!InputActive) return;
            CursorPos += (Woofer.Controller.InputManager.ActiveInputMap.Run.IsPressed() ? 4 : 2)*Woofer.Controller.InputManager.ActiveInputMap.Movement;
        }
        public override void Tick()
        {
            Rectangle viewRect = new Rectangle(Owner.CurrentViewport.Location - new Vector2D(320 / 2 - 16, 180 / 2 - 16), new Size(320-32, 180-32));
            viewRect.Width -= EditorRendering.SidebarWidth * 320 / 1280;

            if (!viewRect.Contains(CursorPos))
            {
                if (CursorPos.X < viewRect.Left) Owner.CurrentViewport.X += (CursorPos.X - viewRect.Left) / 2;
                if (CursorPos.X > viewRect.Right) Owner.CurrentViewport.X += (CursorPos.X - viewRect.Right) / 2;
                if (CursorPos.Y < viewRect.Bottom) Owner.CurrentViewport.Y += (CursorPos.Y - viewRect.Bottom) / 2;
                if (CursorPos.Y > viewRect.Top) Owner.CurrentViewport.Y += (CursorPos.Y - viewRect.Top) / 2;
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            var layer = r.GetLayerGraphics("hi_res_overlay");

            CameraView view = Owner.CurrentViewport;

            System.Drawing.Size screenSize = Woofer.Controller.RenderingUnit.ScreenSize;

            Vector2D pos = CursorPos;
            pos -= view.Location;

            float scale = screenSize.Width / 320f;
            pos *= scale;

            pos.Y *= -1;

            pos.X += screenSize.Width / 2;
            pos.Y += screenSize.Height / 2;

            float size = 11 * 4;

            layer.Draw(r.SpriteManager["editor"], new Rectangle(pos.X - size / 2, pos.Y - size / 2, size, size).ToDrawing(), new System.Drawing.Rectangle(0, 0, 11, 11));
        }

        public override void EventFired(object sender, Event e)
        {
            if (e is BeginModalChangeEvent bmce)
            {
                if (!InputActive) bmce.SystemName = this.SystemName;
                InputActive = false;
            } else if (e is ModalChangeEvent)
            {
                InputActive = true;
            }
        }
    }
}
