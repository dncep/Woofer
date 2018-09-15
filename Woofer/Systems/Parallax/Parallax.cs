using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Parallax
{
    [Component("Parallax")]
    class Parallax : Component
    {
        public string Texture;
        public Rectangle SourceBounds;
        public Vector2D Speed;
        public double Scale;

        public Parallax(string texture, Rectangle sourceBounds, Vector2D speed, double scale)
        {
            Texture = texture;
            SourceBounds = sourceBounds;
            Speed = speed;
            Scale = scale;
        }
    }
}
