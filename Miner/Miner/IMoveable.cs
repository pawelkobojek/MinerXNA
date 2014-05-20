using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Interfejs zapewniający możliwość poruszania się po mapie.
    /// </summary>
    public interface IMoveable
    {
        /// <summary>
        /// Ruch obiektu w kierunku Direction.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek ruchu</param>
        void Move(Direction direction);
    }
}
