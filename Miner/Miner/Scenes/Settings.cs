using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Klasa przechowująca informacje o ustawieniach gry. Jest to singleton.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Referencja na instancję singletona.
        /// </summary>
        private static Settings instance = new Settings();

        /// <summary>
        /// Pole przechowujące informacje o ustawieniach związanych ze sterowaniem.
        /// </summary>
        public Controls Controls { get; set; }
        /// <summary>
        /// Pole przechowujące informacje o ustawieniach związanych z dźwiękiem.
        /// </summary>
        public Sound Sound { get; set; }

        private Settings()
        {
            Controls = new Controls { Action = Keys.Space, Up = Keys.W, Down = Keys.S, Left = Keys.A, Right = Keys.D };
            Sound = new Sound { Music = true, SoundEffects = true };
        }

        /// <summary>
        /// Zwraca, lub tworzy nową instancję singletona.
        /// </summary>
        /// <returns>Instacja singletona.</returns>
        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
            }

            return instance;
        }
    }
}
