using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Scena rozgrywki.
    /// </summary>
    public class GameScene : MenuScene
    {
        /// <summary>
        /// Czas w milisekundach, który upłynął od pojawienia się ostatniego bonusu.
        /// </summary>
        private int bonusSpawnTime = 0;

        private const string FONT_ASSET = "general";

        private SpriteFont font;
        private Vector2 barPosition = Vector2.Zero;

        /// <summary>
        /// Numer ostatniego poziomu.
        /// </summary>
        private const int LAST_LEVEL = 4;

        public GameScene(MinerGame game)
            : base(game)
        {
            font = game.Content.Load<SpriteFont>(FONT_ASSET);

            GenerateLevel(this.Game.GameState.CurrentLevel);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            this.bonusSpawnTime += gameTime.ElapsedGameTime.Milliseconds;

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                PauseMenu newMenu = new PauseMenu(this.Game, this)
                {
                    OldKeyState = this.CurrentKeyState
                };
                MenuManager.GetInstance().CurrentMenu = newMenu;
            }

            if (this.Game.GameState.LevelCompleted)
            {
                if (this.Game.GameState.CurrentLevel == LAST_LEVEL)
                {
                    this.Game.ExitGame();
                    return;
                }

                this.Game.GameState.LevelCompleted = false;
                this.Game.GameState.GatesOpen = false;
                ++this.Game.GameState.CurrentLevel;
                //Console.WriteLine("Current level: {0}", this.GameState.CurrentLevel);
                GenerateLevel(this.Game.GameState.CurrentLevel);
                this.Game.GameState.Miner.Position = new Vector2(0f, 0f);
            }

            foreach (var firstDim in Game.GameState.Map.Fields)
            {
                foreach (var item in firstDim)
                {
                    item.Update(gameTime);
                    if (item.Bonus != null)
                    {
                        item.Bonus.Update(gameTime);
                    }
                }
            }

            if (BonusItem.SpawnTime < this.bonusSpawnTime)
            {
                Random rand = new Random();
                int x = rand.Next(GameMap.DEF_WIDTH);
                int y = 0;
                for (; y < GameMap.DEF_HEIGHT - 1 && Game.GameState.Map.Fields[x][y++].IsEmpty; ) ;

                if (this.Game.GameState.Map.Fields[x][y - 2].Bonus == null ||
                    this.Game.GameState.Map.Fields[x][y - 2].Bonus.GetType() != typeof(Key))
                {
                    this.Game.GameState.Map.Fields[x][y - 2].Bonus = BonusItem.CreateRandomBonus(this.Game, x, y - 2);
                }
                this.bonusSpawnTime = 0;
            }

            this.Game.GameState.Miner.Update(gameTime);

            for (int i = 0; i < this.Game.GameState.Enemies.Count; i++)
            {
                var item = this.Game.GameState.Enemies[i];
                if (item != null)
                {
                    item.Update(gameTime);
                }
            }
            for (int i = 0; i < this.Game.GameState.Bullets.Count; i++)
            {
                var item = this.Game.GameState.Bullets[i];
                if (item != null)
                {
                    item.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, String.Format("Health: {0}/{1}    Fuel: {2}/{3}    Exp: {4}/{5}    Points: {6}    Keys: {7}/{8}",
                this.Game.GameState.Miner.Suit.Durability, this.Game.GameState.Miner.Suit.MaxDurability,
                this.Game.GameState.Miner.Suit.Fuel, this.Game.GameState.Miner.Suit.MaxFuel,
                this.Game.GameState.Miner.Experience, this.Game.GameState.Miner.ExpToNextLevel,
                this.Game.GameState.User.Score,
                this.Game.GameState.Keys.Count, this.Game.GameState.CurrentLevel + 2), barPosition, Color.Black);

            foreach (var firstDim in this.Game.GameState.Map.Fields)
            {
                foreach (var item in firstDim)
                {
                    if (item != null)
                    {
                        item.Draw(spriteBatch);
                    }
                }
            }
            this.Game.GameState.Miner.Draw(spriteBatch);

            foreach (var item in this.Game.GameState.Enemies)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch);
                }
            }

            foreach (var item in this.Game.GameState.Bullets)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Metoda generująca poziom z pliku.
        /// </summary>
        /// <param name="number">Numer poziomu do wygenerowania.</param>
        private void GenerateLevel(int number)
        {
            switch (number)
            {
                case 0:
                    for (int x = 0; x < GameMap.DEF_WIDTH; x++)
                    {
                        for (int y = 0; y < GameMap.DEF_HEIGHT / 2; y++)
                        {
                            this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y, true);
                        }
                        for (int y = GameMap.DEF_HEIGHT / 2; y < GameMap.DEF_HEIGHT - 1; y++)
                        {
                            this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y);
                        }

                        this.Game.GameState.Map.Fields[x][GameMap.DEF_HEIGHT - 1] = new TitanRock(this.Game, x, GameMap.DEF_HEIGHT - 1);
                    }

                    this.Game.GameState.Enemies.Add(new Kosmojopek(this.Game, GameMap.DEF_WIDTH / 2, GameMap.DEF_HEIGHT / 2 - 1));
                    break;
                default:
                    // Generate first level:
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [#][#][#][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][#][#][#][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][#]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][#][#][#][#][#][#][ ][ ][#][ ]
                    // [ ][ ][ ][ ][ ][ ][#][#][#][#][#][#][#][#][ ][ ][ ][ ][ ][ ][#][#][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
                    // [T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T][T]
                    using (StreamReader streamReader = new StreamReader(String.Format("level{0}.txt", number)))
                    {
                        int x = 0;
                        int y = 0;
                        while (streamReader.Peek() >= 0)
                        {
                            string text = streamReader.ReadLine();
                            for (int i = 0; i < text.Length; i += 3)
                            {
                                switch (text[i + 1])
                                {
                                    case '#':
                                        this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y);
                                        break;
                                    case 'K':
                                        this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y);
                                        this.Game.GameState.Map.Fields[x][y].Bonus = new Key(this.Game, x, y);
                                        break;
                                    case 'k':
                                        this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y, true);
                                        this.Game.GameState.Map.Fields[x][y].Bonus = new Key(this.Game, x, y);
                                        break;
                                    case 'T':
                                        this.Game.GameState.Map.Fields[x][y] = new TitanRock(this.Game, x, y);
                                        break;
                                    case 'G':
                                        this.Game.GameState.Map.Fields[x][y] = new SpaceGate(this.Game, x, y);
                                        break;
                                    default:
                                        this.Game.GameState.Map.Fields[x][y] = new Field(this.Game, x, y, true);
                                        break;
                                }
                                x++;
                            }
                            y++;
                            x = 0;
                        }
                        streamReader.Close();
                        this.Game.GameState.Enemies.Add(new Kosmojopek(this.Game, GameMap.DEF_WIDTH / 2, GameMap.DEF_HEIGHT / 2 - 1));
                        this.Game.GameState.Enemies.Add(new Ufolowca(this.Game, GameMap.DEF_WIDTH / 2 - 5, GameMap.DEF_HEIGHT / 2 - 1));
                        this.Game.GameState.Enemies.Add(new WladcaLaserowejDzidy(this.Game, GameMap.DEF_WIDTH / 2 - 5, GameMap.DEF_HEIGHT / 2 - 1));
                        this.Game.GameState.Keys.Clear();
                    }
                    //for (int x = 0; x < GameMap.DEF_WIDTH; x++)
                    //{
                    //    for (int y = 0; y < GameMap.DEF_HEIGHT / 2; y++)
                    //    {
                    //        this.GameState.Map.Fields[x, y] = new Field(this, x, y, true);
                    //    }
                    //}

                    //for (int x = 0; x < GameMap.DEF_WIDTH; x++)
                    //{
                    //    for (int y = GameMap.DEF_HEIGHT / 2 + 3; y < GameMap.DEF_HEIGHT - 1; y++)
                    //    {
                    //        this.GameState.Map.Fields[x, y] = new Field(this, x, y);
                    //    }

                    //    this.GameState.Map.Fields[x, GameMap.DEF_HEIGHT - 1] = new TitanRock(this, x, GameMap.DEF_HEIGHT - 1);
                    //}

                    //for (int x = 0; x < 3; x++)
                    //{
                    //    for (int y = GameMap.DEF_HEIGHT / 2; y < GameMap.DEF_HEIGHT / 2 + 3; y++)
                    //    {
                    //        this.GameState.Map.Fields[x, y] = new Field(this, x, y);
                    //    }
                    //}


                    break;
            }
        }
    }
}
