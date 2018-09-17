using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves;

namespace EntityComponentSystem.Util
{
    [PersistentObject]
    public class Size
    {
        [PersistentProperty]
        public double Width;
        [PersistentProperty]
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
