using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Camera.Shake
{
    [ComponentSystem("camera_shake", ProcessingCycles.Tick),
        Listening(typeof(CameraShakeEvent), typeof(CameraLocationQueryEvent))]
    class CameraShakeSystem : ComponentSystem
    {
        private Vector2D Offset { get; set; }
        private Vector2D OffsetVelocity { get; set; }

        private readonly double timeScale = 64;
        
        public override void Tick()
        {
            for(int i = 0; i < timeScale && (OffsetVelocity.Magnitude > 0 || Offset.Magnitude > 0); i++)
            {
                Offset += OffsetVelocity * Owner.DeltaTime;
                OffsetVelocity += (-Offset * Owner.DeltaTime);
                OffsetVelocity *= 0.995f;
                if(OffsetVelocity.Magnitude < 1e-1 && Offset.Magnitude < 1e-1)
                {
                    OffsetVelocity = Vector2D.Empty;
                    Offset = Vector2D.Empty;
                }

                Woofer.Controller.InputManager.ActiveInputMap.SetVibration((float)(Offset.Magnitude / 256));
            }
        }

        public override void EventFired(object sender, Event e)
        {
            if(e is CameraShakeEvent se)
            {
                OffsetVelocity += se.Motion;
            } else if(e is CameraLocationQueryEvent qe)
            {
                qe.SuggestedLocation += Offset;
            }
        }
    }
}
