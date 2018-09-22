using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("EntityViewSystem", ProcessingCycles.None)]
    class EntityViewSystem : ComponentSystem
    {
    }
}
