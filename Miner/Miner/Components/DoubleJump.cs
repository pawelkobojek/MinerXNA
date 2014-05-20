using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca bonus typu "Podwójny skok"
    /// </summary>
    public class DoubleJump : BonusItem
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/doublejump";

        /// <summary>
        /// Flaga ograniczająca ilość podskoków do dwóch.
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game">Referencja do obiektu gry.</param>
        /// <param name="posX">Pozycja X na mapie.</param>
        /// <param name="posY">Pozycja Y na mapie.</param>
        public DoubleJump(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 15000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.Used = false;
            this.LifeTime = 15000;
            this.RemainingLifeTime = this.LifeTime;
        }


    }
}
