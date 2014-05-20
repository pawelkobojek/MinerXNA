using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Interfejs zapewniający możliwość bycia użytym przez gracza za pomocą przycisku akcji.
    /// </summary>
    public interface IActionUsable
    {
        /// <summary>
        /// Metoda wywoływana po wciśnięciu przez gracza przycisku akcji.
        /// </summary>
        /// <returns>Znaczenie zależne od implementacji.</returns>
        int UseAction();
    }
}
