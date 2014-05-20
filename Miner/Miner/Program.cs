using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (MinerGame game = new MinerGame())
            {
                game.Run();
            }
        }
    }
}

