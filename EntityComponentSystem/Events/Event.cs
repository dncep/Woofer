using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace EntityComponentSystem.Events
{
    public class Event
    {
        public string EventType { get; private set; }
        public Component Sender { get; }

        public Event(string eventType, Component sender)
        {
            EventType = eventType;
            Sender = sender;
        }
    }
}
