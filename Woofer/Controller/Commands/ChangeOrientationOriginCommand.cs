using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Commands;
using EntityComponentSystem.Util;

namespace WooferGame.Controller.Commands
{
    class ChangeOrientationOriginCommand : Command
    {
        public Vector2D newOrigin;

        public ChangeOrientationOriginCommand(Vector2D newOrigin) => this.newOrigin = newOrigin;
    }
}
