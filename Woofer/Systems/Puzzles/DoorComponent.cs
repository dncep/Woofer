using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;

namespace WooferGame.Systems.Puzzles
{
    [Component("door")]
    class DoorComponent : Component
    {
        [PersistentProperty]
        public float MaxOpenDistance = 48 - 8;
        [PersistentProperty]
        public float CurrentOpenDistance = 0;
        [PersistentProperty]
        public float OpeningTime = 0.5f;

        [PersistentProperty]
        public float OpeningDirection = 0;

        [PersistentProperty]
        public bool Toggle = false;
    }
}
