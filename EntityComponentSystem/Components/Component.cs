using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Components
{
    [PersistentObject]
    public abstract class Component
    {
        public Entity Owner { get; set; }
        public string ComponentName { get; private set; }

        private static readonly Dictionary<Type, string> identifierCache = new Dictionary<Type, string>();
        private static Dictionary<string, Type> identifierToTypeMap = null;

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

        public static void PopulateDictionaryWithAssembly(Assembly assembly)
        {
            if (identifierToTypeMap == null) identifierToTypeMap = new Dictionary<string, Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(Component).IsAssignableFrom(type))
                {
                    object[] attributes;
                    if ((attributes = type.GetCustomAttributes(typeof(ComponentAttribute), false)).Length > 0)
                    {
                        string thisIdentifier = (attributes[0] as ComponentAttribute).ComponentName;
                        identifierToTypeMap[thisIdentifier] = type;
                    }
                }
            }
        }

        public static void PopulateIdentifiers()
        {
            if (identifierToTypeMap == null)
            {
                PopulateDictionaryWithAssembly(Assembly.GetExecutingAssembly());
                PopulateDictionaryWithAssembly(Assembly.GetEntryAssembly());
            }
        }

        public static Type TypeForIdentifier(string identifier)
        {
            PopulateIdentifiers();
            return identifierToTypeMap[identifier];
        }

        public static List<string> GetAllIdentifiers()
        {
            PopulateIdentifiers();
            return identifierToTypeMap.Keys.ToList();
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
