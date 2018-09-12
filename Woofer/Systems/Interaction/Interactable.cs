using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;

namespace WooferGame.Systems.Interaction
{
    [Component("interactable")]
    class Interactable : Component
    {
        public long? EntityToActivate;
        public bool InRange = false;

        public Interactable() : this(null)
        {
        }

        public Interactable(long? entityToActivate) => EntityToActivate = entityToActivate;
    }
}
