using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa bazowa dla wszystkich klas reprezentujących bonusy.
    /// </summary>
    [XmlInclude(typeof(DoubleJump))]
    [XmlInclude(typeof(Indestructibility))]
    [XmlInclude(typeof(KeyLocalizer))]
    [XmlInclude(typeof(Laser))]
    [XmlInclude(typeof(MoreEnemies))]
    [XmlInclude(typeof(Fuel))]
    public abstract class BonusItem : GameObject
    {
        /// <summary>
        /// Czas, co który pojawia się nowy bonus danego typu (o ile to możliwe).
        /// </summary>
        public static int SpawnTime = 3000;

        /// <summary>
        /// Czas występowania (w milisekundach).
        /// </summary>
        protected int LifeTime = 30000;

        /// <summary>
        /// Pozostały czas na mapie.
        /// </summary>
        public int RemainingLifeTime { get; set; }

        /// <summary>
        /// Całkowity czas działania.
        /// </summary>
        protected int TotalUsageTime;

        /// <summary>
        /// Pozostały czas działania.
        /// </summary>
        public int RemainingUsageTime { get; set; }

        /// <summary>
        /// Pole na którym znajduje się bonus.
        /// </summary>
        protected Field field;

        /// <summary>
        /// Domyślny konstruktor.
        /// </summary>
        public BonusItem() { }

        /// <summary>
        /// Konstruktor przypisujący pole na mapie.
        /// </summary>
        /// <param name="game">Referencja na obiekt gry.</param>
        /// <param name="posX">Pozycja poziomo (X)</param>
        /// <param name="posY">Pozycja pionowo (Y)</param>
        public BonusItem(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.field = game.GameState.Map.Fields[posX][posY];
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, new Rectangle((int)this.Position.X, (int)this.Position.Y, Field.FieldWidth, Field.FieldHeight), Color.White);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            if (field.Bonus == null)
            {
                // Bonus is taken, do nothing.
                return;
            }
            RemainingLifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (RemainingLifeTime <= 0)
            {
                //Console.WriteLine("Destroyed");
                field.Bonus = null;
            }
        }

        /// <summary>
        /// Tworzy bonus losowego typu.
        /// </summary>
        /// <param name="minerGame">Referencja do obiektu gry.</param>
        /// <param name="x">Pozycja X na mapie</param>
        /// <param name="y">Pozycja Y na mapie</param>
        /// <returns>Bonus losowego typu</returns>
        public static BonusItem CreateRandomBonus(MinerGame minerGame, int x, int y)
        {
            Random rand = new Random();
            switch (rand.Next(6))
            {
                case 0:
                    return new Fuel(minerGame, 35, x, y);
                case 1:
                    return new DoubleJump(minerGame, x, y);
                case 2:
                    return new Indestructibility(minerGame, x, y);
                case 3:
                    return new KeyLocalizer(minerGame, x, y);
                case 4:
                    return new Laser(minerGame, x, y);
                case 5:
                    return new MoreEnemies(minerGame, x, y);
                default:
                    return null;
            }
        }
    }
}
