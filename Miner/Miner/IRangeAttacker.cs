using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Interfejs zapewniający możliwość zadawania obrażeń na odległość.
    /// </summary>
    public interface IRangeAttacker
    {
        /// <summary>
        /// Pole określające wartość obrażeń zadawanych na odległość.
        /// </summary>
        int RangedDamage { get; set; }
        /// <summary>
        /// Metoda wywoływana przez postać atakującą na odległość.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek strzału</param>
        /// <param name="damage">Wartość zadanych obrażeń</param>
        /// <returns>Obiekt pocisku stworzony w wyniku strzału</returns>
        Bullet RangeAttack(ShootDirection direction, int damage);
    }
}
