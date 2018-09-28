using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using EntityComponentSystem.Util.Generics;
using GameInterfaces.Controller;

namespace EntityComponentSystem.ComponentSystems
{
    public class SystemMap : IEnumerable<ComponentSystem>
    {
        private readonly Scene Owner;
        public readonly OrderedDictionary<string, ComponentSystem> Dictionary = new OrderedDictionary<string, ComponentSystem>();

        private List<ComponentSystem> _inputSystems = new List<ComponentSystem>();
        private List<ComponentSystem> _tickSystems = new List<ComponentSystem>();
        private List<ComponentSystem> _renderSystems = new List<ComponentSystem>();

        public SystemMap(Scene owner)
        {
            Owner = owner;
        }

        public ComponentSystem this[string name] => Dictionary[name];

        public int Count => Dictionary.Count;
        public void Add(ComponentSystem system)
        {
            ToAdd.Add(system);
        }

        public void Clear() => Dictionary.Clear();
        public bool Contains(string name) => Dictionary.ContainsKey(name);
        public bool Remove(string name)
        {
            if (!Dictionary.ContainsKey(name)) return false;
            ToRemove.Add(name);
            return true;
        }

        private List<ComponentSystem> ToAdd = new List<ComponentSystem>();
        private List<string> ToRemove = new List<string>();

        private List<Action> QueuedActions = new List<Action>();

        public void QueueOnFlush(Action action) => QueuedActions.Add(action);

        internal void Flush()
        {
            QueuedActions.ForEach(a => a.Invoke());
            QueuedActions.Clear();
            foreach(ComponentSystem system in ToAdd)
            {
                Dictionary.Add(system.SystemName, system);
                system.Owner = Owner;

                if (system.InputProcessing) _inputSystems.Add(system);
                if (system.TickProcessing) _tickSystems.Add(system);
                if (system.RenderProcessing) _renderSystems.Add(system);

                foreach (Entity entity in Owner.Entities)
                {
                    foreach (Component component in entity.Components)
                    {
                        if (system.Watching.Contains(component.ComponentName))
                        {
                            system.AddWatchedComponent(component);
                        }
                    }
                }

                foreach (string eventName in system.Listening)
                {
                    Owner.Events.RegisterEventType(eventName);
                    Owner.Events.EventDictionary[eventName] += system.EventFired;
                }
            }
            ToAdd.Clear();
            foreach(string remove in ToRemove)
            {
                if (!Dictionary.ContainsKey(remove)) continue;
                ComponentSystem system = Dictionary[remove];
                foreach (string eventName in system.Listening)
                {
                    Owner.Events.RegisterEventType(eventName);
                    Owner.Events.EventDictionary[eventName] -= system.EventFired;
                }
                _inputSystems.Remove(system);
                _tickSystems.Remove(system);
                _renderSystems.Remove(system);
                Dictionary.Remove(remove);
            }
            ToRemove.Clear();
        }

        public void UpdateOrder()
        {
            _inputSystems.Clear();
            _tickSystems.Clear();
            _renderSystems.Clear();

            _inputSystems = Dictionary.Values.Where(s => s.InputProcessing).ToList();
            _tickSystems = Dictionary.Values.Where(s => s.TickProcessing).ToList();
            _renderSystems = Dictionary.Values.Where(s => s.RenderProcessing).ToList();
        }

        internal void InvokeInput() => _inputSystems.ForEach((s) =>
        {
            if (Owner.Disposed) return;
            if (!Owner.Controller.Paused || s.PauseProcessing) s.Input();
        });
        internal void InvokeTick() => _tickSystems.ForEach((s) =>
        {
            if (Owner.Disposed) return;
            if (!Owner.Controller.Paused || s.PauseProcessing) s.Tick();
        });
        internal void InvokeRender<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r) => _renderSystems.ForEach((s) => {
            if (Owner.Disposed) return;
            s.Render(r);
        });

        public IEnumerator<ComponentSystem> GetEnumerator() => Dictionary.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Dictionary.GetEnumerator();
    }
}
