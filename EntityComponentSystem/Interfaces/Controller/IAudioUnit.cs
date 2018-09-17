using GameInterfaces.Audio;

namespace GameInterfaces.Controller
{
    public interface IAudioUnit
    {
        ISoundEffect this[string name] { get; }

        void Load(string name);
    }
}
