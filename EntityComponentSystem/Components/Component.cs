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

        protected Component()
        {
            this.ComponentName = GeneralUtil.RequireAttribute<ComponentAttribute>(this.GetType()).ComponentName;
        }

        public virtual void EventFired(object sender, Event e)
        {
        }

        public static string IdentifierOf<T>() where T : Component
        {
            ComponentAttribute attribute = typeof(T).GetCustomAttributes(typeof(ComponentAttribute), false).First() as ComponentAttribute;
            return attribute.ComponentName;
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
