using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems.DeathBarrier;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Pulse;
using WooferGame.Systems.Puzzles;
using WooferGame.Systems.Visual;

namespace WooferGame.Scenes.LevelObjects
{
    class MovableBox : Entity
    {
        public MovableBox(Vector2D pos)
        {
            this.Components.Add(new Spatial(pos));
            this.Components.Add(new Physical());
            this.Components.Add(new SoftBody(new CollisionBox(-8, 0, 16, 16), 4f));
            this.Components.Add(new Renderable(new Sprite("grass", new Rectangle(-8, 0, 16, 16))));
            this.Components.Add(new LevelRenderable());
            this.Components.Add(new AvoidCorners());

            this.Components.Add(new RemoveOnBarrierComponent());
            this.Components.Add(new PulsePushable());
        }
    }
}
