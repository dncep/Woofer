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
    public class Scene : IDisposable
    {
        public readonly IGameController Controller;

        public string Name { get; set; } = "Scene";

        public EntityMap Entities;
        public SystemMap Systems;
        public EventManager Events;

        public CameraView CurrentViewport { get; set; }

        public float DeltaTime { get; private set; } = 0;
        public Random Random { get; set; } = new Random();

        public float FixedDeltaTime = 1 / 60f;

        public Scene(IGameController controller)
        {
            Controller = controller;

            Entities = new EntityMap(this);
            Systems = new SystemMap(this);
            Events = new EventManager();

            Entities.Changed += NotifyEntityChange;
            CurrentViewport = new CameraView();
        }

        public void Draw<TSurface, TSource>(ScreenRenderer<TSurface, TSource> screenRenderer)
        {
            //Console.WriteLine("render");
            Systems.Flush();
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
            Systems.Flush();

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

        public virtual void InvokeInput()
        {
            //Console.WriteLine("input");
            Systems.Flush();
            Systems.InvokeInput();
        }

        public void Destroy()
        {
            List<string> systemNames = Systems.Select(s => s.SystemName).ToList();
            foreach(string systemName in systemNames)
            {
                Systems.Remove(systemName);
            }
            foreach(Entity entity in Entities)
            {
                entity.Remove();
            }
            Entities.Flush();
            Entities.Changed -= NotifyEntityChange;
        }

        #region IDisposable Support
        public bool Disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Destroy();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Entities = null;
                Systems = null;
                Events = null;

                Disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Scene() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
