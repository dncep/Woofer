using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;

namespace EntityComponentSystem.Components
{
    public class ComponentMap : IEnumerable<Component>
    {
        private readonly Entity _owner;
        private readonly Dictionary<string, Component> _dict = new Dictionary<string, Component>();

        public ComponentMap(Entity owner)
        {
            _owner = owner;
        }

        public Component this[string name] => _dict.ContainsKey(name) ? _dict[name] : null;

        public int Count => _dict.Count;
        public void Add(Component component)
        {
            _dict.Add(component.ComponentName, component);
            component.Owner = _owner;
        }
        public void Clear() => _dict.Clear();
        public bool Contains(string name) => _dict.ContainsKey(name);
        public bool Remove(string name) => _dict.Remove(name);

        public IEnumerator<Component> GetEnumerator() => _dict.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

        public T Get<T>() where T : Component
        {
            return _dict[Component.IdentifierOf<T>()] as T;
        }

        public bool Has<T>() where T : Component
        {
            return _dict.ContainsKey(Component.IdentifierOf<T>());
        }
    }
}
