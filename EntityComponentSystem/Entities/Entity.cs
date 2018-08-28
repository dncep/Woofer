using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace EntityComponentSystem.Entities
{
    public class Entity
    {
        private static int NEW_ID = 0;

        public int Id { get; }
        public ComponentMap Components { get; }

        public Entity()
        {
            this.Id = NEW_ID++;
            this.Components = new ComponentMap(this);
        }
    }
}
