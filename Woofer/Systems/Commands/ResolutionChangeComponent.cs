using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves;

namespace WooferGame.Systems.Commands
{
    [Component("resolution_change")]
    class ResolutionChangeComponent : Component
    {
        [PersistentProperty]
        public Size Resolution { get; set; } = new Size(320, 180);
    }
}
