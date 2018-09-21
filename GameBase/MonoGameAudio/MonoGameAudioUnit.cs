using System.Collections.Generic;

using GameInterfaces.Audio;
using GameInterfaces.Controller;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace GameBase.MonoGameAudio
{
    class MonoGameAudioUnit : IAudioUnit
    {
        private readonly ContentManager Content;
        private readonly Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
        private readonly Dictionary<string, Song> Songs = new Dictionary<string, Song>();

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
