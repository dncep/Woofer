using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInterfaces.Input
{
    public enum ButtonState
    {
        Pressed,
        Released
    }

    public static class ButtonStateExtensions
    {
        public static bool IsPressed(this ButtonState state)
        {
            return state == ButtonState.Pressed;
        }
    }
}
