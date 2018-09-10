using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;

namespace WooferGame.Systems.Visual.Animation
{
    [ComponentSystem("animation", ProcessingCycles.Render),
        Watching(typeof(AnimationComponent))]
    class AnimationSystem : ComponentSystem
    {
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
            foreach(AnimationComponent c in WatchedComponents)
            {
                Renderable renderable = c.Owner.Components.Get<Renderable>();
                if (renderable == null) continue;

                foreach(AnimatedSprite anim in c.Animations)
                {
                    if (anim.CurrentFrame < 0 || anim.CurrentFrame >= anim.FrameCount) continue;
                    anim.FrameProgress++;
                    if(anim.FrameProgress >= anim.FrameDurations[anim.CurrentFrame])
                    {
                        anim.FrameProgress = 0;
                        anim.CurrentFrame++;
                        if (anim.CurrentFrame >= anim.FrameCount && anim.Loop)
                        {
                            anim.CurrentFrame = 0;
                        }
                        else Owner.Events.InvokeEvent(new AnimationEndEvent(c, anim));
                    }

                    renderable.Sprites[anim.SpriteIndex].Source = new Rectangle(anim.Origin + (anim.Step * anim.CurrentFrame), anim.FrameSize);
                }
            }
        }
    }
}
