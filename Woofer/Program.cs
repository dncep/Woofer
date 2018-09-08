using System;

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
