using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;
using GameInterfaces.Input;

namespace WooferGame.Input
{
    interface IInputMap
    {
        Vector2D Movement { get; }
        Vector2D Orientation { get; }
        ButtonState Jump { get; }
        ButtonState Pulse { get; }
    }
}
