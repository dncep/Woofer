using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase;
using WooferGame.Controller;

namespace WooferGame
{
    static class Woofer
    {
        public static WooferController Controller;

        [STAThread]
        static void Main(string[] args)
        {
            GameStart.Start(Controller = new WooferController());
        }
    }
}
