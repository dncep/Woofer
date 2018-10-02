using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using WooferGame.Systems.Camera;
using WooferGame.Systems.Interaction;

namespace WooferGame.Systems.Cutscenes
{
    [ComponentSystem("cutscenes", ProcessingCycles.Tick),
        Listening(typeof(ActivationEvent), typeof(CameraLocationQueryEvent))]
    class CutsceneSystem : ComponentSystem
    {
        private long CurrentNode = 0;
        private float Elapsed = 0;

        public override void Tick()
        {
            if(CurrentNode != 0)
            {
                CutsceneNode node = Owner.Entities[CurrentNode]?.Components.Get<CutsceneNode>();
                if (node == null)
                {
                    CurrentNode = 0;
                    return;
                }

                Elapsed += Owner.DeltaTime;
                if(Elapsed > node.Delay + node.Duration)
                {
                    CurrentNode = node.Next;
                    Elapsed -= node.Delay + node.Duration;
                    if(node.Next != 0)
                    {
                        Entity next = Owner.Entities[node.Next];
                        if(next != null && next.Active)
                        {
                            Owner.Events.InvokeEvent(new ActivationEvent(node, next, null));
                        }
                    }
                }
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is ActivationEvent ae && ae.Affected.Components.Get<CutsceneNode>() is CutsceneNode node)
            {
                Elapsed = 0;
                CurrentNode = node.Owner.Id;
            } else if(e is CameraLocationQueryEvent camera)
            {
                if(CurrentNode != 0 && Owner.Entities[CurrentNode] is Entity nodeEntity)
                {
                    node = nodeEntity.Components.Get<CutsceneNode>();
                    Spatial spCur = nodeEntity.Components.Get<Spatial>();
                    if (node == null || spCur == null) return;
                    if (node.Duration == 0) return;
                    Vector2D currentPos = spCur.Position;
                    Vector2D nextPos = Owner.Entities[node.Next]?.Components.Get<Spatial>()?.Position ?? spCur.Position;

                    Vector2D delta = nextPos - currentPos;

                    float interpolation = 0;

                    if(Elapsed > node.Delay)
                    {
                        if(node.Interpolate)
                        {
                            interpolation = (float)Math.Sin((Math.PI/2)*(Elapsed - node.Delay) / node.Duration);
                        } else
                        {
                            interpolation = (Elapsed - node.Delay) / node.Duration;
                        }
                    }

                    camera.SuggestedLocation = currentPos + interpolation * delta;
                }
            }
        }
    }
}
