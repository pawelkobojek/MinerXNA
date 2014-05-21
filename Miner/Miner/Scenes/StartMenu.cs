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
        /// Napis tytułowy
        /// </summary>
        private const string LABEL_TITLE = "Podaj imie:";

        /// <summary>
        /// Podane przez użytkownika imię gracza.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Czcionka napisów na przycisku.
        /// </summary>
        protected SpriteFont font;

        /// <summary>
        /// Ścieżka do czcionki użytej na tej scenie.
        /// </summary>
        private const string FONT_ASSET = "general";

        /// <summary>
        /// Pozycja tekstu wpisywanego przez uzytkownika.
        /// </summary>
        protected Vector2 inputNaturalPosition = Vector2.Zero;

        public StartMenu(MinerGame game)
            : base(game)
        {
            PlayerName = "";


            font = this.Game.Content.Load<SpriteFont>(FONT_ASSET);

            MenuItems.Add(new MenuItem(this, new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                    Game.GraphicsDevice.Viewport.Height / 2 - 150, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), LABEL_TITLE, null));

            MenuItems[currentlySelected].Selected = false;
        }

        /// <summary>
        /// Sprawdza, czy wciśnięty przycisk jest literą.
        /// </summary>
        /// <param name="key">Wciśnięty klucz</param>
        /// <returns></returns>
        private static bool IsKeyAChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.GetPressedKeys().Length == 0)
            {
                OldKeyState = keyState;
                return;
            }
            Keys pressedKey = Keyboard.GetState().GetPressedKeys()[0];
            if (IsKeyAChar(pressedKey) && !OldKeyState.IsKeyDown(pressedKey))
            {
                PlayerName += pressedKey.ToString();
            }

            if (pressedKey == Keys.Enter)
            {
                this.Game.GameState.User = new User
                {
                    Name = PlayerName,
                    Score = 0
                };

                MainMenu newMenu = new MainMenu(this.Game)
                {
                    OldKeyState = keyState
                };
                MenuManager.GetInstance().CurrentMenu = newMenu;
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

            Vector2 textDim = font.MeasureString(PlayerName);
            inputNaturalPosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - textDim.X / 2,
                Game.GraphicsDevice.Viewport.Height / 2 - textDim.Y / 2);

            spriteBatch.DrawString(font, PlayerName, inputNaturalPosition, Color.Black);
        }
    }
}
