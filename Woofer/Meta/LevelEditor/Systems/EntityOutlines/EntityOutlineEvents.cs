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
    class BeginOutline : Event
    {
        public IOutline Outline;

        public BeginOutline(IOutline outline) : base(null)
        {
            this.Outline = outline;
        }
    }
    [Event("remove_outline")]
    class RemoveOutline : Event
    {
        public IOutline Outline;

        public RemoveOutline(IOutline outline) : base(null)
        {
            this.Outline = outline;
        }
    }
    [Event("clear_outline")]
    class ClearEntityOutlines : Event
    {
        public ClearEntityOutlines() : base(null)
        {
        }
    }
}
