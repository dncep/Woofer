using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Events;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;

namespace EntityComponentSystem.ComponentSystems
{
    public abstract class ComponentSystem
    {
        private static readonly Dictionary<Type, ComponentSystemAttribute> attributeCache = new Dictionary<Type, ComponentSystemAttribute>();

        public string SystemName { get; private set; }

        private const byte _input = 0b001;
        private const byte _tick = 0b010;
        private const byte _render = 0b100;

        public Scene Owner { get; set; }
        public string[] Watching { get; private set; } = new string[0];
        public string[] Listening { get; private set; } = new string[0];
        private readonly ProcessingCycles ProcessingCycles = ProcessingCycles.None;

        public bool InputProcessing => (ProcessingCycles & ProcessingCycles.Input) != ProcessingCycles.None;
        public bool TickProcessing => (ProcessingCycles & ProcessingCycles.Tick) != ProcessingCycles.None;
        public bool RenderProcessing => (ProcessingCycles & ProcessingCycles.Render) != ProcessingCycles.None;

        protected List<Component> WatchedComponents = new List<Component>();
        
        public ComponentSystem()
        {
            try
            {
                ComponentSystemAttribute attr = AttributeOf(this.GetType());
                this.SystemName = attr.SystemName;
                this.ProcessingCycles = attr.ProcessingCycles;
                this.Watching = attr.Watching;
                this.Listening = attr.Listening;
            }
            catch (Exception ex)
            {
                throw new AttributeException(
                    $"Required attribute of class 'ComponentSystemAttribute' not found in derived class '{GetType()}'", ex);
            }
        }

        public virtual void RemoveWatchedComponent(Component component)
        {
            WatchedComponents.Remove(component);
        }

        public virtual void AddWatchedComponent(Component component)
        {
            WatchedComponents.Add(component);
        }

        public virtual void EventFired(object sender, Event e)
        {
        }

        public virtual void Input()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
        }

        public static string IdentifierOf<T>() where T : ComponentSystem
        {
            return IdentifierOf(typeof(T));
        }

        public static string IdentifierOf(Type type)
        {
            return AttributeOf(type).SystemName;
        }

        private static ComponentSystemAttribute AttributeOf(Type type)
        {
            if (!attributeCache.ContainsKey(type))
            {
                attributeCache[type] = (type.GetCustomAttributes(typeof(ComponentSystemAttribute), false).First() as ComponentSystemAttribute);
                attributeCache[type].Watching = (type.GetCustomAttributes(typeof(WatchingAttribute), false).FirstOrDefault() as WatchingAttribute)?.Watching ?? new string[0];
                attributeCache[type].Listening = (type.GetCustomAttributes(typeof(ListeningAttribute), false).FirstOrDefault() as ListeningAttribute)?.Listening ?? new string[0];
            }
            return attributeCache[type];
        }
    }

    public sealed class ComponentSystemAttribute : Attribute
    {
        public readonly string SystemName;
        public readonly ProcessingCycles ProcessingCycles;
        public string[] Watching;
        public string[] Listening;

        public ComponentSystemAttribute(string systemName, ProcessingCycles cycles)
        {
            SystemName = systemName;
            ProcessingCycles = cycles;
        }
    }

    public sealed class WatchingAttribute : Attribute
    {
        public readonly string[] Watching;
        public WatchingAttribute(params Type[] watching)
        {
            if (!watching.All(t => typeof(Component).IsAssignableFrom(t))) throw new ArgumentException("watching");
            Watching = watching.Select(t => Component.IdentifierOf(t)).ToArray();
        }
    }

    public sealed class ListeningAttribute : Attribute
    {
        public readonly string[] Listening;
        public ListeningAttribute(params Type[] listening)
        {
            if (!listening.All(t => typeof(Event).IsAssignableFrom(t))) throw new ArgumentException("listening");
            Listening = listening.Select(t => Event.IdentifierOf(t)).ToArray();
        }
    }

    [Flags]
    public enum ProcessingCycles
    {
        None = 0,
        Input = 1,
        Tick = 2,
        Render = 4
    }
}
