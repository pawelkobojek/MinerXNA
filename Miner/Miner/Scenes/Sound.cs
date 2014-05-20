using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Klasa przechowująca informacje związane z dźwiękiem.
    /// </summary>
    public class Sound
    {
        /// <summary>
        /// Pole przechowujące informacje o wł./wył. efektów dźwiękowych.
        /// </summary>
        public bool SoundEffects { get; set; }
        /// <summary>
        /// Pole przechowujące informacje o wł./wył. muzyki.
        /// </summary>
        public bool Music { get; set; }
    }
}
