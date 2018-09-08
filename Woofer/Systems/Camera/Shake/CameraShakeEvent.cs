using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Camera.Shake
{
    [Event("camera_shake")]
    class CameraShakeEvent : Event
    {
        public double Strength { get; set; }

        public CameraShakeEvent(Component sender, double strength) : base(sender)
        {
            this.Strength = strength;
        }

    }
}
