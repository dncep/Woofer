using System;
using EntityComponentSystem.Scenes;

namespace GameInterfaces.Controller
{
    public interface IGameController
    {
        Scene ActiveScene { get; }
        IRenderingUnit RenderingUnit { get; }

        IInputUnit InputUnit { get; set; }

        void Initialize();
        void Tick(TimeSpan timeSpan, TimeSpan elapsedGameTime);
        void Input();
    }
}
