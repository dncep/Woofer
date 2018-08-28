using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase;
using Woofer.Controller;

namespace Woofer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GameStart.Start(new WooferController());
        }
    }
}
