using System;
using System.Collections.Generic;

namespace WooferGame.Input
{
    class InputMapManager
    {
        private readonly List<IInputMap> inputMaps = new List<IInputMap>();
        public IInputMap ActiveInputMap { get; private set; } = null;

        public InputMapManager()
        {

        }

        public void Add(IInputMap inputMap)
        {
            inputMaps.Add(inputMap);
        }

        public void Remove(IInputMap inputMap)
        {
            inputMaps.Remove(inputMap);
        }

        private void SwitchInputMap(IInputMap inputMap)
        {
            ActiveInputMap = inputMap;
            Console.WriteLine("New input: " + inputMap.Name);
        }

        public void Tick()
        {
            if (ActiveInputMap == null && inputMaps.Count >= 1) SwitchInputMap(inputMaps[0]);

            foreach(IInputMap inputMap in inputMaps)
            {
                if(inputMap != ActiveInputMap && inputMap.IsBeingUsed)
                {
                    SwitchInputMap(inputMap);
                    break;
                }
            }
        }
    }
}
