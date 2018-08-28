using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Util
{
    public class Size
    {
        public int Width;
        public int Height;

        public Size() : this(0, 0)
        {

        }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
