using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves;

namespace WooferGame.Controller.Game
{
    [PersistentObject]
    public class GameData
    {
        [PersistentProperty]
        public bool HasWoofer = true;
        [PersistentProperty]
        public int MaxEnergy = 160;
        [PersistentProperty]
        public int MaxHealth = 4;

        public override string ToString()
        {
            return $"Has Woofer: {HasWoofer}, Max Energy: {MaxEnergy}, Max Health: {MaxHealth}";
        }
    }
}
