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
    /// Klasa reprezentująca bonus "Niezniszczalność".
    /// </summary>
    public class Indestructibility : BonusItem
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/indestructibility";

        public Indestructibility(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 10000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.LifeTime = 20000;
            this.RemainingLifeTime = this.LifeTime;
        }
    }
}
