using System;
using System.IO;
using GameBase;

using WooferGame.Controller;

namespace WooferGame
{
    public static class Woofer
    {
        public static WooferController Controller;

        public static readonly string DirectoryPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"Woofer/");

        [STAThread]
        static void Main(string[] args)
        {
            GameStart.Start(Controller = new WooferController());
        }
    }
}
