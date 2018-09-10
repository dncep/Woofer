using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;

namespace WooferGame.Systems.Linking
{
    [Component("following")]
    class FollowingComponent : Component
    {
        public long FollowedID { get; set; }

        public FollowingComponent(long followedID) => FollowedID = followedID;

        public FollowingComponent(Entity followed) : this(followed.Id)
        {

        }
    }
}
