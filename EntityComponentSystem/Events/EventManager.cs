using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;

namespace EntityComponentSystem.Events
{
    public class EventManager
    {
        private event EventHandler<Event> Dummy;

        public readonly Dictionary<string, EventHandler<Event>> EventDictionary = new Dictionary<string, EventHandler<Event>>();

        public EventHandler<Event> this[string name] => EventDictionary[name];

        public EventManager()
        {
        }

        public void RegisterEventType(string type)
        {
            if (!EventDictionary.ContainsKey(type)) EventDictionary.Add(type, Dummy);
        }

        public void InvokeEvent(Event evt)
        {
            EventDictionary[evt.EventType].Invoke(this, evt);
        }

        public void InvokeEvent(Event evt, Entity target, object sender)
        {
            foreach (Component component in target.Components)
            {
                if (component.ListenedEvents.Contains(evt.EventType))
                {
                    component.EventFired(sender, evt);
                }
            }
        }

        public void InvokeEvent(Event evt, List<Entity> targets, object sender)
        {
            foreach (Entity target in targets)
            {
                InvokeEvent(evt, target, sender);
            }
        }
    }
}
