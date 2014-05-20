using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Interfejs zapewniający możliwość zadawania obrażeń w zwarciu.
    /// </summary>
    public interface IMeleeAttacker
    {
        /// <summary>
        /// Metoda wywoływana przez postać atakującą w zwarciu.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek ataku</param>
        /// <returns>Wartość zadanych obrażeń</returns>
        int MeleeAttack(Direction direction);
        /// <summary>
        /// Pole określające wartość obrażeń zadawanych w zwarciu.
        /// </summary>
        int MeleeDamage { get; set; }
    }
}
