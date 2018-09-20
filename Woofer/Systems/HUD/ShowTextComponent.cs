using EntityComponentSystem.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooferGame.Systems.HUD
{
    [Component("show_text")]

    class ShowTextComponent : Component 
    {
        public string Text;
        public double Duration;

        public ShowTextComponent(string text, double duration)
        {
            Text = text;
            Duration = duration;
        }
    }
}
