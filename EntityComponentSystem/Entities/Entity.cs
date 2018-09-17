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
        public string Name { get; set; }

        public Scene Owner { get; set; }

        internal long _id;
        internal bool _is_id_set = false;
        public long Id => (_is_id_set ? _id : throw new InvalidOperationException("Id is still not assigned"));
        public ComponentMap Components { get; }
        public bool Active { get; set; } = true;

        public Entity()
        {
            this.Components = new ComponentMap(this);
            this.Name = this.GetType().Name;
        }

        public void Remove()
        {
            Active = false;
            Owner.Entities.Remove(Id);
        }

        public virtual void Initialize()
        {

        }
    }
}
