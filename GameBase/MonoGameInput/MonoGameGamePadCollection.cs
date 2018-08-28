using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInterfaces.Input.GamePad;
using Microsoft.Xna.Framework.Input;

namespace GameBase.MonoGameInput
{
    public class MonoGameGamePadCollection : IGamePadCollection
    {
        private IGamePad[] gamePads;
        public IGamePad this[int index]
        {
            get
            {
                if (gamePads[index] == null) gamePads[index] = new MonoGameGamePad(index);
                return gamePads[index];
            }
        }

        public MonoGameGamePadCollection()
        {
            gamePads = new IGamePad[GamePad.MaximumGamePadCount];
        }
    }
}
