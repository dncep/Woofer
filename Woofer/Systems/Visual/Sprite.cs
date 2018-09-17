using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Interfaces.Visuals;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace WooferGame.Systems.Visual
{
    [PersistentObject]
    class Sprite
    {
        [PersistentProperty]
        public string Texture;
        [PersistentProperty]
        public Rectangle Destination;
        [PersistentProperty]
        public Rectangle Source;

        [PersistentProperty]
        public DrawMode DrawMode = DrawMode.Normal;

        public Sprite()
        {
        }

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
