using Microsoft.Xna.Framework;
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
                                using (Stream stream = openFile.OpenFile())
                                {
                                    XmlSerializer xml = new XmlSerializer(typeof(GameState));
                                    this.Game.GameState = (GameState)xml.Deserialize(stream);
                                    this.Game.GameState.Miner.Suit = new Nanosuit();
                                    stream.Close();
                                }

                                this.Game.GameState.Miner.game = this.Game;

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
