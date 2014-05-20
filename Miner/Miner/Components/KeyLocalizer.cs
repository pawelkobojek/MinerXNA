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
    /// Klasa reprezentująca bonus "Lokalizator kluczy"
    /// </summary>
    public class KeyLocalizer : BonusItem, IActionUsable
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Bonuses/key_localizer";

        /// <summary>
        /// Czas w milisekundach, który trzeba odczekać pomiędzy kolejnymi użyciami bonusu.
        /// </summary>
        private const int COOLDOWN = 3000;

        /// <summary>
        /// Liczba milisekund, która upłynęła od ostatniego użycia bonusu.
        /// </summary>
        private int currentCooldown = 0;

        public KeyLocalizer(MinerGame minerGame, int x, int y)
            : base(minerGame, x, y)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.TotalUsageTime = 15000;
            this.RemainingUsageTime = this.TotalUsageTime;
            this.LifeTime = 20000;
            this.RemainingLifeTime = this.LifeTime;
        }

        /// <summary>
        /// Implementacja interfejsu IActionUsable. Metoda wywoływana po wciśnięciu przez gracza przycisku akcji.
        /// </summary>
        /// <returns>Zawsze 0</returns>
        public int UseAction()
        {
            if (currentCooldown > 0)
            {
                //Console.WriteLine("Remaining cooldown: {0}", currentCooldown);
                return 0;
            }

            int x = (int)Math.Round(game.GameState.Miner.Position.X / Field.FieldWidth);
            int y = (int)Math.Round(game.GameState.Miner.Position.Y / Field.FieldHeight);
            //Console.WriteLine("({0}, {1})", x, y);

            if (x >= GameMap.DEF_WIDTH || y + 3 >= GameMap.DEF_HEIGHT)
            {
                return 0;
            }

            for (int i = y + 1; i <= y + 3; i++)
            {
                game.GameState.Map.Fields[x][i].Reveal();
            }

            currentCooldown = COOLDOWN;

            return 0;
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
    }
}
