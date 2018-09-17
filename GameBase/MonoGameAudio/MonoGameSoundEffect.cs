using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInterfaces.Audio;
using Microsoft.Xna.Framework.Audio;

namespace GameBase.MonoGameAudio
{
    class MonoGameSoundEffect : ISoundEffect
    {
        public bool Looping
        {
            get => sound.IsLooped;
            set => sound.IsLooped = value;
        }
        public float Pan
        {
            get => sound.Pan;
            set => sound.Pan = value;
        }
        public float Volume
        {
            get => sound.Volume;
            set => sound.Volume = value;
        }
        public float Pitch
        {
            get => sound.Pitch;
            set => sound.Pitch = value;
        }

        private readonly SoundEffectInstance sound;

        public MonoGameSoundEffect(SoundEffectInstance sound)
        {
            this.sound = sound;
        }

        public void Play() => sound.Play();
        public void Pause() => sound.Pause();
        public void Resume() => sound.Resume();
        public void Stop() => sound.Stop();
    }
}
