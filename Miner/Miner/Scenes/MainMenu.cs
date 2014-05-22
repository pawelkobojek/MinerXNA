using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca menu główne.
    /// </summary>
    public class MainMenu : MenuScene
    {
        /// <summary>
        /// Tekst na przycisku rozpoczynającym grę.
        /// </summary>
        private const string START = "Start";
        /// <summary>
        /// Tekst na przycisku pozwalającym na wczytanie gry.
        /// </summary>
        private const string LOAD_GAME = "Wczytaj";
        /// <summary>
        /// Tekst na przycisku prowadzącym do listy najlepszych wyników.
        /// </summary>
        private const string HIGH_SCORES = "Najlepsze wyniki";
        /// <summary>
        /// Tekst na przycisku prowadzącym do ustawień.
        /// </summary>
        private const string SETTINGS = "Ustawienia";
        /// <summary>
        /// Tekst na przycisku wyjścia z gry.
        /// </summary>
        private const string EXIT = "Wyjscie";

        /// <summary>
        /// Tekst błędu wczytania pliku.
        /// </summary>
        private const string ERROR_READING_FILE = "Could not read the file.";

        public MainMenu(MinerGame game)
            : base(game)
        {
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 - 300, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), START, (menu) =>
                    {
                        // Following line protects new menu from instantly reading previous key as pressed.
                        DifficultyMenu newMenu = new DifficultyMenu(this.Game, this)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        MenuManager.GetInstance().CurrentMenu = newMenu;
                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 - 150, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), LOAD_GAME, (menu) =>
                    {
                        System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();

                        if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                this.Game.Content.Unload();

                                using (Stream stream = openFile.OpenFile())
                                {
                                    XmlSerializer xml = new XmlSerializer(typeof(GameState));
                                    this.Game.GameState = (GameState)xml.Deserialize(stream);
                                    this.Game.GameState.Miner.Suit = new Nanosuit();
                                    stream.Close();
                                }

                                this.Game.GameState.Miner.game = this.Game;

                                foreach (var item in this.Game.GameState.Enemies)
                                {
                                    item.game = this.Game;
                                }

                                foreach (var item in this.Game.GameState.Bullets)
                                {
                                    item.game = this.Game;
                                }

                                this.Game.Content.Load<SpriteFont>("general");
                                this.Game.Content.Load<Texture2D>(Kosmojopek.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Ufolowca.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(WladcaLaserowejDzidy.ASSET_NAME);
                                //this.Content.Load<Texture2D>(PoteznySultan.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Miner.ASSET_NAME);
                                //this.Content.Load<Texture2D>(CosmicMatter.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(DoubleJump.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Field.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Fuel.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Indestructibility.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Key.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(KeyLocalizer.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Laser.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(MoreEnemies.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(SolidRock.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(SpaceGate.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(TitanRock.ASSET_NAME);
                                this.Game.Content.Load<Texture2D>(Bullet.ASSET_NAME);

                                this.Game.GameState.Miner.LoadContent();

                                foreach (var item in this.Game.GameState.Enemies)
                                {
                                    item.LoadContent();
                                }

                                foreach (var item in this.Game.GameState.Bullets)
                                {
                                    item.LoadContent();
                                }

                                foreach (var item in this.Game.GameState.Map.Fields)
                                {
                                    foreach (var field in item)
                                    {
                                        field.game = this.Game;
                                        field.LoadContent();
                                    }
                                }

                                GameScene newScene = new GameScene(this.Game)
                                {
                                    OldKeyState = this.CurrentKeyState
                                };
                                MenuManager.GetInstance().CurrentMenu = newScene;
                            }
                            catch (Exception exc)
                            {
                                System.Windows.Forms.MessageBox.Show(ERROR_READING_FILE + exc.Message);
                            }
                        }

                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), HIGH_SCORES, (menu) =>
                    {
                        // Following line protects new menu from instantly reading previous key as pressed.
                        HighScoreMenu newMenu = new HighScoreMenu(this.Game, this)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        MenuManager.GetInstance().CurrentMenu = newMenu;
                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + 150, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), SETTINGS, (menu) =>
                    {
                        SettingsMenu newMenu = new SettingsMenu(this.Game, this)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        MenuManager.GetInstance().CurrentMenu = newMenu;
                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + 300, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), EXIT, (menu) =>
                    {
                        menu.Game.ExitGame();
                    }));

            MenuItems[currentlySelected].Selected = true;
        }
    }
}
