using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace EntityComponentSystem.Components
{
    public abstract class Component
    {
        public Entity Owner { get; set; }
        public string ComponentName { get; protected set; }
        public string[] ListenedEvents { get; protected set; } = new string[0];

        public virtual void EventFired(object sender, Event e)
        {
        }
    }
}
