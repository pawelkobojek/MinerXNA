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
    /// Klasa reprezentująca bonus "Paliwo".
    /// </summary>
    public class Fuel : BonusItem
    {
        /// <summary>
        /// Ilość paliwa (w jednostkach), które zawiera ten bonus.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/fuel";

        private Fuel() { }

        public Fuel(int amount)
        {
            this.Amount = amount;
        }

        public Fuel(MinerGame game, int amount, int posX, int posY)
            : base(game, posX, posY)
        {
            this.Amount = amount;
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.LifeTime = 30000;
            this.RemainingLifeTime = this.LifeTime;
        }

        /// <summary>
        /// Zwraca ścieżkę do obrazka. Zawsze zwraca ASSET_NAME.
        /// </summary>
        /// <returns>Ścieżkę do obrazka.</returns>
        public override string GetAssetName()
        {
            return ASSET_NAME;
        }
    }
}
