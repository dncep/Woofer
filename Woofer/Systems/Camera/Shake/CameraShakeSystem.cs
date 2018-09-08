using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera.Shake
{
    [ComponentSystem("camera_shake")]
    class CameraShakeSystem : ComponentSystem
    {
        private Vector2D Offset { get; set; }
        private Vector2D OffsetVelocity { get; set; }

        private readonly double timeScale = 64;

        public CameraShakeSystem()
        {
            TickProcessing = true;
            Listening = new string[] {
                Event.IdentifierOf<CameraShakeEvent>(),
                Event.IdentifierOf<CameraLocationQueryEvent>()
            };
        }
        
        public override void Tick()
        {
            for(int i = 0; i < timeScale && (OffsetVelocity.Magnitude > 0 || Offset.Magnitude > 0); i++)
            {
                Offset += OffsetVelocity * Owner.DeltaTime;
                OffsetVelocity += (-Offset * Owner.DeltaTime);
                OffsetVelocity *= 0.995;
                if(OffsetVelocity.Magnitude < 1e-1 && Offset.Magnitude < 1e-1)
                {
                    OffsetVelocity = Vector2D.Empty;
                    Offset = Vector2D.Empty;
                }
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is CameraShakeEvent se)
            {
                OffsetVelocity += new Vector2D(0, -se.Strength);
            } else if(e is CameraLocationQueryEvent qe)
            {
                Owner.Events.InvokeEvent(new CameraLocationResponseEvent(null, Offset));
            }
        }
    }
}
