using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Events;
using WooferGame.Systems.Camera.Shake;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Physics;

namespace WooferGame.Systems.Environment
{
    [ComponentSystem("glass_breaking")]
    class GlassBreakingSystem : ComponentSystem
    {
        public GlassBreakingSystem()
        {
            Listening = new string[] { Event.IdentifierOf<ActivationEvent>() };
        }

        public override void EventFired(object sender, Event re)
        {
            if(re is ActivationEvent e && e.Affected.Components.Has<BreakableGlassComponent>())
            {
                e.Affected.Components.Get<RigidBody>().Bounds = new CollisionBox[0];
                Owner.Events.InvokeEvent(new CameraShakeEvent(re.Sender, 16));
            }
        }
    }
}
