using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooferGame.Input
{
    class InputTimeframe
    {
        private bool Consumed = false;
        private int Effectiveness = 0;
        private int Timeframe = 5; //ticks

        public InputTimeframe(int timeframe) => Timeframe = timeframe;

        public void RegisterPressed()
        {
            if(!Consumed)
            {
                Effectiveness = Timeframe;
                Consumed = true;
            }
        }

        public bool Execute()
        {
            bool returnValue = Effectiveness > 0;
            Effectiveness = 0;
            return returnValue;
        }

        public void RegisterUnpressed()
        {
            Consumed = false;
        }

        public void Tick()
        {
            if (Effectiveness > 0) Effectiveness--;
        }
    }
}
