using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa przechowująca pojedynczy wpis wyniku. Zawiera informacje o wyniku oraz jego autorze.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Imię gracza, który osiągnął zapisany w obiekcie tej klasy wynik.
        /// </summary>
        public string PlayerName { get; set; }
        /// <summary>
        /// Wartość wyniku.
        /// </summary>
        public int Score { get; set; }
    }
}
