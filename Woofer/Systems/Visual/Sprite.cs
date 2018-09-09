using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Visual
{
    class Sprite
    {
        public string Texture;
        public Rectangle Destination;
        public Rectangle Source;

        public Sprite(string texture, Rectangle destination) : this(texture, destination, null)
        {
        }

        public Sprite(string texture, Rectangle destination, Rectangle source)
        {
            Texture = texture;
            Destination = destination;
            Source = source;
        }
    }
}
