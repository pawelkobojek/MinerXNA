using Microsoft.Xna.Framework;
using Miner.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa odpowiedzialna za menu wyboru poziomu trudności
    /// </summary>
    public class DifficultyMenu : MenuScene
    {
        /// <summary>
        /// Napis na etykiecie tytułowej.
        /// </summary>
        private const string CHOOSE_DIFFICULTY = "Wybierz poziom trudnosci";
        /// <summary>
        /// Napis na przyicsku wyboru łatwego poziomu trudności.
        /// </summary>
        private const string EASY = "Latwy";
        /// <summary>
        /// Napis na przyicsku wyboru średniego poziomu trudności.
        /// </summary>
        private const string MEDIUM = "Sredni";
        /// <summary>
        /// Napis na przyicsku wyboru trudnego poziomu trudności.
        /// </summary>
        private const string HARD = "Trudny";

        public DifficultyMenu(MinerGame game, MenuScene previous)
            : base(game, previous)
        {
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 - 300, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), EASY, (menu) =>
                    {
                        // Following line protects new menu from instantly reading previous key as pressed.
                        GameScene newScene = new GameScene(this.Game)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        menu.Game.GameState.Difficulty = DifficultyLevel.Easy;
                        MenuManager.GetInstance().CurrentMenu = newScene;

                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 - 150, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), MEDIUM, (menu) =>
                    {
                        // Following line protects new menu from instantly reading previous key as pressed.
                        GameScene newScene = new GameScene(this.Game)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        menu.Game.GameState.Difficulty = DifficultyLevel.Medium;
                        MenuManager.GetInstance().CurrentMenu = newScene;
                    }));
            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), HARD, (menu) =>
                    {
                        // Following line protects new menu from instantly reading previous key as pressed.
                        GameScene newScene = new GameScene(this.Game)
                        {
                            OldKeyState = this.CurrentKeyState
                        };
                        menu.Game.GameState.Difficulty = DifficultyLevel.Hard;
                        MenuManager.GetInstance().CurrentMenu = newScene;
                    }));
            MenuItems[currentlySelected].Selected = true;
        }
    }
}
