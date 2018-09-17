using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Util;

namespace WooferGame.Controller.Commands
{
    class OrientationOriginChangeCommand : Command
    {
        public Vector2D NewOrigin;

        public OrientationOriginChangeCommand(Vector2D newOrigin) => this.NewOrigin = newOrigin;
    }
}
