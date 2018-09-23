using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using GameInterfaces.Controller;
using WooferGame.Common;
using WooferGame.Input;
using WooferGame.Meta.LevelEditor.Systems.EntityView;

namespace WooferGame.Meta.LevelEditor.Systems.CursorModes
{
    [ComponentSystem("move_cursor_mode", ProcessingCycles.Input | ProcessingCycles.Tick | ProcessingCycles.Render, ProcessingFlags.Pause),
        Listening(typeof(StartMoveModeEvent))]
    class MoveCursorModeSystem : ComponentSystem
    {
        private EditorCursorSystem CursorSystem = null;
        private bool ModalActive = false;
        private List<IMemberSummary> Members = new List<IMemberSummary>();

        public override bool ShouldSave => false;

        public override void Input()
        {
            if (!ModalActive) return;
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            if(Editor.SelectTimeframe.Execute())
            {
                foreach(IMemberSummary member in Members)
                {
                    member.SetValue(CursorSystem.CursorPos);
                }
                Members.Clear();
                ModalActive = false;
                Owner.Events.InvokeEvent(new RequestModalChangeEvent(null));
            }
        }

        public override void Tick()
        {
            if(CursorSystem == null) CursorSystem = Owner.Systems["editor_cursor"] as EditorCursorSystem;

            if(ModalActive)
            {
                foreach (IMemberSummary member in Members)
                {
                    member.SetValue(CursorSystem.CursorPos);
                }
            }
        }

        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            if (!ModalActive) return;
            var layer = r.GetLayerGraphics("hi_res_overlay");

            new TextUnit("Moving").Render(r, layer, new System.Drawing.Point(0, 0), 2);
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is StartMoveModeEvent start)
            {
                ModalActive = true;
                Members.Add(start.Member);
            } if(e is ModalChangeEvent changed)
            {
                changed.Valid = false;
                Owner.Events.InvokeEvent(new ForceModalChangeEvent("editor_cursor", null));
                CursorSystem.SwitchToModal = changed.OldSystem;
            }
        }
    }

    [Event("start_move_mode")]
    class StartMoveModeEvent : Event
    {
        public IMemberSummary Member;

        public StartMoveModeEvent(IMemberSummary member) : base(null) => Member = member;
    }
}
