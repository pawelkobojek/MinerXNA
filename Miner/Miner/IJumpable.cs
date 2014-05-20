using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Interfejs gwarantujący możliwość skakania.
    /// </summary>
    public interface IJumpable
    {
        /// <summary>
        /// Pole określające wartość pionowo skierowanego impulsu nadanego w momencie skoku.
        /// </summary>
        float JumpImpulse { get; set; }
        /// <summary>
        /// Metoda odpowiadająca za wykonanie przez postać skoku poprzez nadanie pionowego impulsu o wartości równej JumpImpulse.
        /// </summary>
        void Jump();
    }
}
