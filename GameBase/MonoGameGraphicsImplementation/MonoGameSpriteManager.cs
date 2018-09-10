using System;
using System.Collections.Generic;

using GameInterfaces.GraphicsInterface;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameBase.MonoGameGraphicsImplementation
{
    public class MonoGameSpriteManager : ISpriteManager<Texture2D>
    {
        private readonly Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();

        private ContentManager content;
        
        public Texture2D this[string name] {
            get => sprites[name];
            set => sprites[name] = value;
        }

        public MonoGameSpriteManager(ContentManager content)
        {
            this.content = content;
        }

        public void LoadSprite(string name)
        {
            sprites[name] = content.Load<Texture2D>(name);
        }
    }
}
