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
        public float Width;
        [PersistentProperty]
        public float Height;

        public Size() : this(0, 0)
        {

        }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
