using System;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Scenes;

namespace GameInterfaces.Controller
{
    public interface IGameController
    {
        Scene ActiveScene { get; }
        IRenderingUnit RenderingUnit { get; }
        IInputUnit InputUnit { get; set; }
        IAudioUnit AudioUnit { get; set; }
        bool Paused { get; set; }

        void Initialize();
        void Tick(TimeSpan timeSpan, TimeSpan elapsedGameTime);
        void Input();

        void CommandFired(Command command);
    }
}
