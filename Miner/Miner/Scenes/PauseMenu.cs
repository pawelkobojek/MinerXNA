using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    /// Klasa odpowiedzialna za menu dostępne w czasie pauzy.
    /// </summary>
    public class PauseMenu : MenuScene
    {
        #region constants
        /// <summary>
        /// Tekst na przycisku wznawiającym grę.
        /// </summary>
        private const string RESUME = "Wznow";
        /// <summary>
        /// Tekst na przycisku pozwalającym na zapisanie gry.
        /// </summary>
        private const string SAVE_GAME = "Zapisz gre";
        /// <summary>
        /// Tekst na przycisku pozwalającym na wczytanie gry.
        /// </summary>
        private const string LOAD_GAME = "Wczytaj gre";
        /// <summary>
        /// Tekst na przycisku wyświetlającym instrukcje.
        /// </summary>
        private const string INSTRUCTIONS = "Instrukcje";
        /// <summary>
        /// Tekst na przycisku prowadzącym do ustawień.
        /// </summary>
        private const string SETTINGS = "Ustawienia";
        /// <summary>
        /// Tekst na przycisku wyjścia do menu głównego.
        /// </summary>
        private const string EXIT_MAIN_MENU = "Wyjscie do menu glownego";
        /// <summary>
        /// Tekst na przycisku wyjścia z gry.
        /// </summary>
        private const string EXIT_GAME = "Wyjscie z gry";
        /// <summary>
        /// Tekst błędu wczytania pliku.
        /// </summary>
        private const string ERROR_READING_FILE = "Could not read the file.";

        /// <summary>
        /// Stała określająca domyślną wysokośc przycisku w tej scenie.
        /// </summary>
        private new const int MENU_ITEM_HEIGHT = 75;
        /// <summary>
        /// Stała określająca przerwę między przyciskami w tej scenie.
        /// </summary>
        private new const int GAP = 40;

        #endregion

        public PauseMenu(MinerGame game)
            : this(game, null)
        {

        }

        public PauseMenu(MinerGame game, MenuScene previous)
            : base(game, previous)
        {
            int start = -400;
            int pos = start;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), RESUME, (menu) =>
                    {
                        this.GoBack();
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), SAVE_GAME, (menu) =>
                    {
                        using (FileStream fs = new FileStream("save1.xml", FileMode.Create))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(GameState));
                            xml.Serialize(fs, this.Game.GameState);
                        }
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), LOAD_GAME, (menu) =>
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
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), INSTRUCTIONS, (menu) =>
                    {

                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), SETTINGS, (menu) =>
                    {
                        SettingsMenu newMenu = new SettingsMenu(this.Game, this)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        MenuManager.GetInstance().CurrentMenu = newMenu;
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), EXIT_MAIN_MENU, (menu) =>
                    {
                        MainMenu newMenu = new MainMenu(this.Game)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        MenuManager.GetInstance().CurrentMenu = newMenu;
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 + pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), EXIT_GAME, (menu) =>
                    {
                        menu.Game.ExitGame();
                    }));
            MenuItems[currentlySelected].Selected = true;
        }
    }
}
