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
    /// Klasa reprezentująca bonus typu "Laser".
    /// </summary>
    public class Laser : BonusItem, IActionUsable
    {
        /// <summary>
        /// Stała będąca domyślną wartością obrażeń zadawanych przez laser.
        /// </summary>
        public const int LASER_DAMAGE = 10;

        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/laser";

        /// <summary>
        /// Czas w milisekundach, który trzeba odczekać pomiędzy kolejnymi użyciami bonusu.
        /// </summary>
        private const int COOLDOWN = 1000;

        /// <summary>
        /// Liczba milisekund, która upłynęła od ostatniego użycia bonusu.
        /// </summary>
        private int currentCooldown = 0;

        private Laser() { }

        public Laser(MinerGame game)
        {
            this.field = new Field();
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 15000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.LifeTime = 15000;
            this.RemainingLifeTime = this.LifeTime;
            this.game = game;
        }

        public Laser(MinerGame minerGame, int x, int y)
            : base(minerGame, x, y)
        {
            this.sprite = minerGame.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 15000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.LifeTime = 15000;
            this.RemainingLifeTime = this.LifeTime;
        }

        /// <summary>
        /// Implementacja interfejsu IActionUsable. Metoda wywoływana po wciśnięciu przez gracza przycisku akcji.
        /// </summary>
        /// <returns>Obrażenia zadane przez laser.</returns>
        public int UseAction()
        {
            if (currentCooldown > 0)
            {
                return 0;
            }
            Miner miner = game.GameState.Miner;
            game.GameState.Bullets.Add(miner.RangeAttack(miner.LastMoveDirection, LASER_DAMAGE));
            currentCooldown = COOLDOWN;
            return LASER_DAMAGE;
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentCooldown > 0)
            {
                currentCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            }
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
