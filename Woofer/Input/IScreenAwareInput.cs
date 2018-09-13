using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace WooferGame.Input
{
    interface IScreenAwareInput
    {
        void SetOrientationOrigin(Vector2D origin);
    }
}
