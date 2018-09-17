using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Linking
{
    [ComponentSystem("following_system", ProcessingCycles.Tick),
        Watching(typeof(FollowingComponent))]
    class FollowingSystem : ComponentSystem
    {
        public override void Tick()
        {
            foreach(FollowingComponent following in WatchedComponents)
            {
                if(following.FollowedID != 0 && Owner.Entities[following.FollowedID] is Entity followed)
                {
                    Spatial sp = followed.Components.Get<Spatial>();
                    if (sp == null) continue;
                    FollowedComponent fc = followed.Components.Get<FollowedComponent>();

                    following.Owner.Components.Get<Spatial>().Position = sp.Position + (fc != null ? fc.Offset : Vector2D.Empty);
                }
            }
        }
    }
}
