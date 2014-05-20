using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa odpowiedzialna za menu startowe (logowania).
    /// </summary>
    public class StartMenu : MenuScene
    {
        /// <summary>
        /// Podane przez użytkownika imię gracza.
        /// </summary>
        public string PlayerName { get; set; }

        public StartMenu(MinerGame game)
            : base(game)
        {

            //menuItems.Add(new MenuItem(game, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - HEIGHT,
            //        Game.GraphicsDevice.Viewport.Height / 2 - 300, WIDTH, HEIGHT), "yes"));

            //menuItems.Add(new MenuItem(game, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - HEIGHT,
            //        Game.GraphicsDevice.Viewport.Height / 2 - 150, WIDTH, HEIGHT), "no"));

            MenuItems[currentlySelected].Selected = true;
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.S) && !OldKeyState.IsKeyDown(Keys.S))
            {
                MenuItems[currentlySelected].Selected = false;

                currentlySelected = (currentlySelected + 1 >= MenuItems.Count) ? 0 : currentlySelected + 1;

                MenuItems[currentlySelected].Selected = true;
            }
            else if (keyState.IsKeyDown(Keys.W) && !OldKeyState.IsKeyDown(Keys.W))
            {
                MenuItems[currentlySelected].Selected = false;

                currentlySelected = (currentlySelected - 1 < 0) ? MenuItems.Count - 1 : currentlySelected - 1;

                MenuItems[currentlySelected].Selected = true;
            }

            OldKeyState = keyState;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            foreach (var item in MenuItems)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}
