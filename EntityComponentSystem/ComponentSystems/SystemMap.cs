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
using GameInterfaces.Controller;

namespace EntityComponentSystem.ComponentSystems
{
    public class SystemMap : IEnumerable<ComponentSystem>
    {
        private readonly Scene Owner;
        private readonly Dictionary<string, ComponentSystem> _dict = new Dictionary<string, ComponentSystem>();

        private readonly List<ComponentSystem> _inputSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> _tickSystems = new List<ComponentSystem>();
        private readonly List<ComponentSystem> _renderSystems = new List<ComponentSystem>();

        public SystemMap(Scene owner)
        {
            Owner = owner;
        }

        public ComponentSystem this[string name] => _dict[name];

        public int Count => _dict.Count;
        public void Add(ComponentSystem system)
        {
            _dict.Add(system.SystemName, system);
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

        public void Clear() => _dict.Clear();
        public bool Contains(string name) => _dict.ContainsKey(name);
        public bool Remove(string name)
        {
            if (!_dict.ContainsKey(name)) return false;
            ComponentSystem system = _dict[name];
            foreach (string eventName in system.Listening)
            {
                Owner.Events.RegisterEventType(eventName);
                Owner.Events.EventDictionary[eventName] -= system.EventFired;
            }
            _dict.Remove(name);
            return true;
        }

        internal void InvokeInput() => _inputSystems.ForEach((s) =>
        {
            if (!Owner.Controller.Paused || s.PauseProcessing) s.Input();
        });
        internal void InvokeTick() => _tickSystems.ForEach((s) =>
        {
            if(!Owner.Controller.Paused || s.PauseProcessing) s.Tick();
        });
        internal void InvokeRender<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r) => _renderSystems.ForEach((s) => s.Render(r));

        public IEnumerator<ComponentSystem> GetEnumerator() => _dict.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    }
}
