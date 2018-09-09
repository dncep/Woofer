using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Util
{
    public class Size
    {
        public double Width;
        public double Height;

        public Size() : this(0, 0)
        {

        }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
