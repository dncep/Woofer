using GameInterfaces.Input;

namespace WooferGame.Input
{
    class InputTimeframe
    {
        protected bool Consumed = false;
        protected int Effectiveness = 0;
        protected int Timeframe = 5; //ticks

        public InputTimeframe(int timeframe) => Timeframe = timeframe;

        public virtual void RegisterPressed()
        {
            if(!Consumed)
            {
                Effectiveness = Timeframe;
                Consumed = true;
            }
        }

        public virtual bool Execute()
        {
            bool returnValue = Effectiveness > 0;
            Effectiveness = 0;
            return returnValue;
        }

        public virtual void RegisterUnpressed()
        {
            Consumed = false;
        }

        public void RegisterState(ButtonState state)
        {
            if (state.IsPressed()) RegisterPressed();
            else RegisterUnpressed();
        }

        public virtual void Tick()
        {
            if (Effectiveness > 0) Effectiveness--;
        }
    }

    class InputRepeatingTimeframe
    {
        protected int Repetitions = 0;
        protected int Delay;
        protected int Period;

        protected int Pressed = 0;

        public InputRepeatingTimeframe(int delay, int period)
        {
            this.Delay = delay;
            this.Period = period;
        }

        public bool Execute()
        {
            bool returnValue = (Repetitions == 0 && Pressed == 1) || (Repetitions > 0 && Pressed == Period);
            if(returnValue && Repetitions != 0)
            {
                Repetitions++;
                Pressed = 0;
            }
            return returnValue;
        }
        public void RegisterPressed()
        {
            Pressed++;
            if((Repetitions == 0 && Pressed > Delay) || (Repetitions > 0 && Pressed > Period))
            {
                Repetitions++;
                Pressed = 0;
            }
        }
        public void RegisterUnpressed()
        {
            Pressed = 0;
            Repetitions = 0;
        }

        public void RegisterState(ButtonState state)
        {
            if (state.IsPressed()) RegisterPressed();
            else RegisterUnpressed();
        }
    }
}
