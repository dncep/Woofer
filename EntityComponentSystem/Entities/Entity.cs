using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Scenes;

namespace EntityComponentSystem.Entities
{
    public class Entity
    {
        private static long NEW_ID = 0;

        public Scene Owner { get; set; }

        public long Id { get; }
        public ComponentMap Components { get; }
        public bool Active { get; set; } = true;

        public Entity()
        {
            this.Id = NEW_ID++;
            this.Components = new ComponentMap(this);
        }

        public void Remove()
        {
            Active = false;
            Owner.Entities.Remove(Id);
        }
    }
}
