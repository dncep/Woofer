using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Visual.Animation;

namespace WooferGame.Systems.Visual.Particles
{
    class CloudParticle : Entity
    {
        public CloudParticle(Vector2D pos) : this(pos, 0)
        {

        }

        public CloudParticle(Vector2D pos, int delay)
        {
            Components.Add(new Spatial(pos));
            Components.Add(new Renderable(new Sprite("particles", new Rectangle(-4.5, -2, 9, 4), new Rectangle(0, 0, 9, 4))));
            Components.Add(new LevelRenderable());
            Components.Add(new AnimationComponent(new AnimatedSprite(0, new Rectangle(0, 0, 9, 4), new Vector2D(9, 0), 6, 4) { FrameProgress = -(delay + 1) }));
            Components.Add(new ParticleComponent());
            Components.Add(new Physical() { GravityMultiplier = 0.1 });
        }

        public override void Initialize()
        {
            Components.Get<Physical>().Velocity = new Vector2D(Owner.Random.NextDouble() * 16, 0).Rotate(Owner.Random.NextDouble() * 2 * Math.PI);
        }
    }

    class AmberParticle : Entity
    {
        public AmberParticle(Vector2D pos) : this(pos, 0)
        {
        }

        public AmberParticle(Vector2D pos, int delay)
        {
            Components.Add(new Spatial(pos));
            Components.Add(new Renderable(new Sprite("particles", new Rectangle(-4.5, -2, 9, 4), new Rectangle(0, 4, 9, 4))));
            Components.Add(new LevelRenderable());
            Components.Add(new AnimationComponent(new AnimatedSprite(0, new Rectangle(0, 4, 9, 4), new Vector2D(9, 0), 6, 4) { FrameProgress = -(delay + 1) }));
            Components.Add(new ParticleComponent());
            Components.Add(new Physical() { GravityMultiplier = 0.1 });
        }

        public override void Initialize()
        {
            Components.Get<Physical>().Velocity = new Vector2D(Owner.Random.NextDouble() * 16, 0).Rotate(Owner.Random.NextDouble() * 2 * Math.PI);
        }
    }

    class PopParticle : Entity
    {
        public PopParticle(Vector2D pos) : this(pos, 0)
        {
        }

        public PopParticle(Vector2D pos, int delay)
        {
            Components.Add(new Spatial(pos));
            Components.Add(new Renderable(new Sprite("particles", new Rectangle(-4, -4, 8, 8), new Rectangle(0, 8, 8, 8))));
            Components.Add(new LevelRenderable());
            Components.Add(new AnimationComponent(new AnimatedSprite(0, new Rectangle(0, 8, 8, 8), new Vector2D(8, 0), 8, 4) { FrameProgress = -(delay + 1) }));
            Components.Add(new ParticleComponent());
        }
    }
}
