﻿using System;
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
using WooferGame.Input;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("editor_cursor", ProcessingCycles.Input | ProcessingCycles.Tick | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(MoveCursorEvent))]
    class EditorCursorSystem : ComponentSystem
    {
        private Vector2D _cursorPos;
        internal Vector2D CursorPos
        {
            get => BoundToGrid ? new Vector2D(8 * (int)Math.Round(_cursorPos.X / 8), 8 * (int)Math.Round(_cursorPos.Y / 8)) : _cursorPos;
            set => _cursorPos = value;
        }
        internal bool BoundToGrid = true;
        private bool InputActive = true;

        public string SwitchToModal = "entity_list";

        public override bool ShouldSave => false;

        public override void Input()
        {
            if (!InputActive) return;
            if (!Woofer.Controller.Paused) return;

            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            _cursorPos += (inputMap.Run.IsPressed() ? 4 : 2)*Woofer.Controller.InputManager.ActiveInputMap.Movement;
            if(BoundToGrid && !inputMap.Interact.IsPressed())
            {
                _cursorPos = CursorPos;
            }
            BoundToGrid = inputMap.Interact.IsPressed();
        }
        public override void Tick()
        {
            if (!Woofer.Controller.Paused) return;
            Rectangle viewRect = new Rectangle(Owner.CurrentViewport.Location - new Vector2D(320 / 2 - 16, 180 / 2 - 16), new Size(320-32, 180-32));
            viewRect.Width -= EditorRendering.SidebarWidth * 320 / 1280;

            if (!viewRect.Contains(CursorPos))
            {
                if (_cursorPos.X < viewRect.Left) Owner.CurrentViewport.X += (_cursorPos.X - viewRect.Left) / 2;
                if (_cursorPos.X > viewRect.Right) Owner.CurrentViewport.X += (_cursorPos.X - viewRect.Right) / 2;
                if (_cursorPos.Y < viewRect.Bottom) Owner.CurrentViewport.Y += (_cursorPos.Y - viewRect.Bottom) / 2;
                if (_cursorPos.Y > viewRect.Top) Owner.CurrentViewport.Y += (_cursorPos.Y - viewRect.Top) / 2;
            }
        }
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!Woofer.Controller.Paused) return;
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
                bmce.SystemName = SwitchToModal;
                InputActive = false;
            } else if (e is ModalChangeEvent ce)
            {
                SwitchToModal = ce.OldSystem;
                InputActive = true;
            } else if(e is MoveCursorEvent move)
            {
                CursorPos = move.Position;
                Owner.CurrentViewport.Location = CursorPos;
            }
        }
    }

    [Event("force_move_cursor")]
    public class MoveCursorEvent : Event
    {
        public Vector2D Position;

        public MoveCursorEvent(Vector2D position) : base(null) => Position = position;
    }
}
