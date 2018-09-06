using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Components
{
    public abstract class Component
    {
        public Entity Owner { get; set; }
        public string ComponentName { get; private set; }

        private static readonly Dictionary<Type, string> identifierCache = new Dictionary<Type, string>();

        protected Component()
        {
            try
            {
                this.ComponentName = IdentifierOf(this.GetType());
            }
            catch (Exception ex) {
                throw new AttributeException(
                    $"Required attribute of class 'ComponentAttribute' not found in derived class '{GetType()}'", ex);
            }
        }

        public virtual void EventFired(object sender, Event e)
        {
        }

        public static string IdentifierOf<T>() where T : Component
        {
            return IdentifierOf(typeof(T));
        }

        public static string IdentifierOf(Type type)
        {
            if (!identifierCache.ContainsKey(type)) identifierCache[type] = (type.GetCustomAttributes(typeof(ComponentAttribute), false).First() as ComponentAttribute).ComponentName;
            return identifierCache[type];
        }
    }

    public sealed class ComponentAttribute : Attribute
    {
        public readonly string ComponentName;

        public ComponentAttribute(string componentName)
        {
            ComponentName = componentName;
        }
    }
}
