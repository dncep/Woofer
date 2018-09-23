using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [Component("entity_delegate")]
    class EntityDelegate : Component
    {
        [PersistentProperty]
        public long Id;
        [PersistentProperty]
        public bool Selected = false;

        public EntityDelegate() : this(0)
        {
        }

        public EntityDelegate(long id) => Id = id;
    }
}
