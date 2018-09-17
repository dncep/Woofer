using System.Collections.Generic;

using GameInterfaces.Audio;
using GameInterfaces.Controller;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameBase.MonoGameAudio
{
    class MonoGameAudioUnit : IAudioUnit
    {
        private readonly ContentManager Content;
        private readonly Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();

        public ISoundEffect this[string name] => new MonoGameSoundEffect(Sounds[name].CreateInstance());

        public MonoGameAudioUnit(ContentManager content)
        {
            this.Content = content;
        }

        public void Load(string name)
        {
            Sounds[name] = Content.Load<SoundEffect>(name);
        }
    }
}
