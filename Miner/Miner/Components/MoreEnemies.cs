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
    /// Klasa reprezentująca bonus typu "Zwiększona liczba wrogów".
    /// </summary>
    public class MoreEnemies : BonusItem
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/more_enemies";

        public MoreEnemies(MinerGame minerGame, int x, int y)
            : base(minerGame, x, y)
        {
            this.sprite = minerGame.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 15000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.LifeTime = 15000;
            this.RemainingLifeTime = this.LifeTime;
        }
    }
}
