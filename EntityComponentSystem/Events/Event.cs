using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Events
{
    public class Event
    {
        public string EventName { get; private set; }
        public Component Sender { get; }

        public Event(Component sender)
        {
            Sender = sender;

            this.EventName = GeneralUtil.RequireAttribute<EventAttribute>(this.GetType()).EventName;
        }

        public static string IdentifierOf<T>() where T : Event
        {
            EventAttribute attribute = typeof(T).GetCustomAttributes(typeof(EventAttribute), false).First() as EventAttribute;
            return attribute.EventName;
        }
    }

    public sealed class EventAttribute : Attribute
    {
        public readonly string EventName;

        public EventAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
