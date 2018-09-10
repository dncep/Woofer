using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using WooferGame.Systems.Visual.Animation;

namespace WooferGame.Systems.Visual.Particles
{
    [ComponentSystem("particles"),
        Listening(typeof(AnimationEndEvent))]
    class ParticleSystem : ComponentSystem
    {
        public override void EventFired(object sender, Event evt)
        {
            if(evt is AnimationEndEvent ae)
            {
                if (!ae.Component.Owner.Components.Has<ParticleComponent>()) return;
                ae.Component.Animations.Remove(ae.Animation);
                if (ae.Component.Animations.Count == 0) ae.Component.Owner.Remove();
            }
        }
    }
}
