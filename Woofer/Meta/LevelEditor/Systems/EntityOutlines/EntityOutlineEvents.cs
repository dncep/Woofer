using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Meta.LevelEditor.Systems.EntityOutlines
{
    [Event("begin_outline")]
    class BeginEntityOutline : Event
    {
        public Entity Entity;

        public BeginEntityOutline(Entity entity, Component sender) : base(sender)
        {
            this.Entity = entity;
        }
    }
    [Event("clear_outline")]
    class ClearEntityOutlines : Event
    {
        public ClearEntityOutlines(Component sender) : base(sender)
        {
        }
    }
}
