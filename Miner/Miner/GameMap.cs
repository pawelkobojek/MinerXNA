using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca mapę gry.
    /// </summary>
    public class GameMap
    {
        /// <summary>
        /// Stała określająca szerokość mapy (w polach).
        /// </summary>
        public const int DEF_WIDTH = 24;
        /// <summary>
        /// Stała określająca wysokość mapy (w polach).
        /// </summary>
        public const int DEF_HEIGHT = 16;

        /// <summary>
        /// Tablica wielkości DEF_WIDTH x DEF_HEIGHT stanowiąca rzeczywistą mapę gry.
        /// </summary>
        public Field[][] Fields { get; set; }

        public GameMap()
        {
            Fields = new Field[DEF_WIDTH][];
            for (int i = 0; i < DEF_WIDTH; i++)
            {
                Fields[i] = new Field[DEF_HEIGHT];
            }
        }
    }
}
