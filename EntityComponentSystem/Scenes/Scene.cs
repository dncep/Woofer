using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;

namespace EntityComponentSystem.Scenes
{
    public class Scene
    {
        public readonly EntityMap Entities;
        public readonly SystemMap Systems;
        public readonly SpriteSet Sprites;
        public readonly EventManager Events;

        public CameraView CurrentViewport { get; }

        public float DeltaTime { get; private set; } = 0;
        public float FixedDeltaTime = 1 / 60f;

        public Scene()
        {
            Entities = new EntityMap(this);
            Systems = new SystemMap(this);
            Sprites = new SpriteSet();
            Events = new EventManager();

            Entities.Changed += NotifyEntityChange;
            CurrentViewport = new CameraView();

        }

        public void Draw<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer)
        {
            //Console.WriteLine("render");
            Systems.InvokeRender(screenRenderer);
        }

        protected virtual void Tick()
        {

        }

        public void InvokeTick(TimeSpan deltaTime, TimeSpan fixedTime)
        {
            //Console.WriteLine("ticks");
            DeltaTime = (float)(deltaTime.TotalMilliseconds / 1000d);
            //Console.WriteLine($"DeltaTime: {DeltaTime}");
            //FixedDeltaTime = (float)(fixedTime.TotalMilliseconds / 1000d);
            Entities.Flush();

            Systems.InvokeTick();

            Tick();
        }

        private void NotifyEntityChange(EntityChangedEventArgs e)
        {
            foreach(Component component in e.Entity.Components)
            {
                NotifyComponentChange(new ComponentChangedEventArgs(component, e.WasRemoved));
            }
            if(e.WasRemoved)
            {
                e.Entity.Components.Changed -= NotifyComponentChange;
            } else
            {
                e.Entity.Components.Changed += NotifyComponentChange;
            }
        }

        private void NotifyComponentChange(ComponentChangedEventArgs e)
        {
            foreach (ComponentSystem system in Systems)
            {
                if (system.Watching.Contains(e.Component.ComponentName))
                {
                    if (e.WasRemoved) system.RemoveWatchedComponent(e.Component);
                    else system.AddWatchedComponent(e.Component);
                }
            }
        }

        public void InvokeInput()
        {
            //Console.WriteLine("input");
            Systems.InvokeInput();
        }
    }
}
