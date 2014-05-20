using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca użytkownika.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Imię gracza
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Aktualny wynik gracza.
        /// </summary>
        public int Score { get; set; }
    }
}
