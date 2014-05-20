using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Klasa przechowująca konfigurację klawiszy.
    /// </summary>
    public class Controls
    {
        /// <summary>
        /// Przycisk "w górę".
        /// </summary>
        public Keys Up { get; set; }
        /// <summary>
        /// Przycisk "w dół".
        /// </summary>
        public Keys Down { get; set; }
        /// <summary>
        /// Przycisk "w lewo".
        /// </summary>
        public Keys Left { get; set; }
        /// <summary>
        /// Przycisk "w prawo".
        /// </summary>
        public Keys Right { get; set; }
        /// <summary>
        /// Przycisk akcji.
        /// </summary>
        public Keys Action { get; set; }
    }
}
