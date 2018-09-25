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

        [PersistentProperty("texture")]
        public string _texture;
        public string Texture
        {
            get
            {
                if ((Modifiers & Mod_InputType) != 0) return Woofer.Controller.InputManager.ActiveInputMap.IconSpritesheet;
                return _texture;
            }
            set => _texture = value;
        }
        [PersistentProperty]
        public Rectangle Destination;
        [PersistentProperty]
        public byte Modifiers = Mod_None;
        [PersistentProperty]
        public Rectangle Source;

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
