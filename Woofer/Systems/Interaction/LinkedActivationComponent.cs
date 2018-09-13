using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Interaction
{
    [Component("linked_activation")]
    class LinkedActivationComponent : Component
    {
        public List<long> EntitiesToActivate = new List<long>();

        public LinkedActivationComponent(params long[] entitiesToActivate) => EntitiesToActivate = entitiesToActivate.ToList();
    }
}
