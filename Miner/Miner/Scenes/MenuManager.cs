using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa odpowiedzialna za zarządzanie aktualnie wyświetlaną sceną. Jest singletonem.
    /// </summary>
    public class MenuManager
    {
        /// <summary>
        /// Instacja singletona.
        /// </summary>
        private static MenuManager instance = new MenuManager();

        /// <summary>
        /// Aktualnie wyświetlana scena.
        /// </summary>
        public MenuScene CurrentMenu { get; set; }

        private MenuManager()
        {
            CurrentMenu = null;
        }

        /// <summary>
        /// Zwraca, lub tworzy nową instancję singletona.
        /// </summary>
        /// <returns>Instacja singletona.</returns>
        public static MenuManager GetInstance()
        {
            if (instance == null)
            {
                instance = new MenuManager();
            }

            return instance;
        }
    }
}
