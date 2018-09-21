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
        public const byte Mod_None = 0;
        public const byte Mod_InputType = 1;

        [PersistentProperty]
        public string Texture;
        [PersistentProperty]
        public Rectangle Destination;
        [PersistentProperty("source")]
        public Rectangle _source;
        [PersistentProperty]
        public byte Modifiers = Mod_None;

        public Rectangle Source {
            get
            {
                if (_source == null) return null;
                if ((Modifiers & Mod_InputType) != 0) _source.X = Woofer.Controller.InputManager.ActiveInputMap.ButtonIconOffset;
                return _source;
            }
            set => _source = value;
        }

        [PersistentProperty]
        public DrawMode DrawMode = DrawMode.Normal;

        public float Opacity = 1f;

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
