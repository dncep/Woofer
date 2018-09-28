using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;
using WooferGame.Systems.Visual;

namespace WooferGame.Systems.Generators
{
    [Component("tiling_generator")]
    class TilingGenerator : Component
    {
        public Rectangle MaxBounds = new Rectangle();
        public List<Sprite> Sprites = new List<Sprite>();
        public bool Preview { get; set; } = false;
        public bool Generate { get; set; } = false;
    }
}
