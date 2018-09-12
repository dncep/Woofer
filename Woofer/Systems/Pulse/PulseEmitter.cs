using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems.Interaction;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Timer;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Pulse
{
    class PulseEmitter : Entity
    {
        public PulseEmitter(Vector2D pos, Vector2D direction, double strength, double reach)
        {
            this.Components.Add(new Spatial(pos));
            this.Components.Add(new Physical() { GravityMultiplier = 0 });
            this.Components.Add(new RigidBody(new CollisionBox(-8, -8, 16, 16)));
            this.Components.Add(new Renderable(new Sprite("brick", new Rectangle(-8, -8, 16, 16)),
                new Sprite("gui", new Rectangle(8, 8, 9, 9), new Rectangle(9, 0, 9, 9))));
            this.Components.Add(new LevelRenderable());
            
            this.Components.Add(new PulseEmitterComponent(direction, strength, reach));
            //this.Components.Add(new TimerComponent(1));

            this.Components.Add(new Interactable());
        }
    }
}
