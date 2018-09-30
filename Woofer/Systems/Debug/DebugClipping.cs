using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using GameInterfaces.Input;
using WooferGame.Input;
using WooferGame.Systems.Camera;

namespace WooferGame.Systems.Debug
{
    [ComponentSystem("debug_clipping", ProcessingCycles.Input),
        Watching(typeof(DebugClippable)),
        Listening(typeof(CameraLocationQueryEvent))]
    class DebugClipping : ComponentSystem
    {
        public override void Input()
        {
            IInputMap inputMap = Woofer.Controller.InputManager.ActiveInputMap;

            foreach (DebugClippable clippable in WatchedComponents)
            {
                if (clippable.Enabled)
                {
                    clippable.CameraLocation += inputMap.DebugMovement.Normalize() * 2;
                    if (inputMap.Debug.Consume())
                    {
                        clippable.Enabled = false;
                        clippable.Owner.Components.Get<Spatial>().Position = clippable.CameraLocation;
                    }
                }
                else
                {
                    if (inputMap.Debug.Consume())
                    {
                        clippable.Enabled = true;
                        clippable.CameraLocation = Owner.CurrentViewport.Location;
                    }
                }
            }
        }

        public override void EventFired(object sender, Event evt)
        {
            if(evt is CameraLocationQueryEvent qe)
            {
                foreach(DebugClippable clippable in WatchedComponents)
                {
                    if (clippable.Enabled)
                    {
                        qe.SuggestedLocation = clippable.CameraLocation;
                    }
                    break;
                }
            }
        }
    }
}
