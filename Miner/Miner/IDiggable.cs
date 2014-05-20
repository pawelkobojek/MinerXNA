using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Interfejs gwarantujący możliwość wykonania kopania.
    /// </summary>
    public interface IDiggable
    {
        /// <summary>
        /// Metoda odpowiadająca za kopanie.
        /// </summary>
        void Dig();
    }
}
