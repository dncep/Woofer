using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Player.Animation
{
    [Component("player_animation")]
    class PlayerAnimation : Component
    {
        public bool Initialized = false;
        public string SpritesheetName;
        public bool LastLookedRight = true;

        public PlayerAnimation(string spritesheetName) => SpritesheetName = spritesheetName;

        public int WalkAnimationProgress { get; internal set; }
    }
}
