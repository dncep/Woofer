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
            Entities = new EntityMap();
            Systems = new SystemMap(this);
            Sprites = new SpriteSet();
            Events = new EventManager();

            Entities.Changed += new EntityChangedEventHandler(NotifyEntityChange);
            CurrentViewport = new CameraView();
        }

        public void Draw<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer)
        {
            Systems.InvokeRender(screenRenderer);
        }

        protected virtual void Tick()
        {

        }

        public void InvokeTick(TimeSpan deltaTime, TimeSpan fixedTime)
        {
            DeltaTime = (float)(deltaTime.TotalMilliseconds / 1000d);
            FixedDeltaTime = (float)(fixedTime.TotalMilliseconds / 1000d);
            Entities.Flush();

            Systems.InvokeTick();

            Tick();
        }

        private void NotifyEntityChange(EntityChangedEventArgs e)
        {
            foreach (Component component in e.Entity.Components)
            {
                foreach (string eventType in component.ListenedEvents)
                {
                    Events.RegisterEventType(eventType);
                    if (e.WasRemoved) Events.EventDictionary[eventType] -= component.EventFired;
                    else Events.EventDictionary[eventType] += component.EventFired;
                }

                foreach (ComponentSystem system in Systems)
                {
                    if (system.Watching.Contains(component.ComponentName))
                    {
                        if (e.WasRemoved) system.RemoveWatchedComponent(component);
                        else system.AddWatchedComponent(component);
                    }
                }
            }
        }

        public void InvokeInput() => Systems.InvokeInput();
    }
}
