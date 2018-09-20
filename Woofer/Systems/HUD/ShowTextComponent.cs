using EntityComponentSystem.Components;
using EntityComponentSystem.Saves;
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
        [PersistentProperty]
        public string Text;
        [PersistentProperty]
        public double Duration;

        public ShowTextComponent() : this("", 5)
        {
        }

        public ShowTextComponent(string text, double duration)
        {
            Text = text;
            Duration = duration;
        }
    }
}
