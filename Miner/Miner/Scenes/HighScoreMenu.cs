using Microsoft.Xna.Framework;
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
    /// Scena menu najlepszych wyników.
    /// </summary>
    public class HighScoreMenu : MenuScene
    {
        /// <summary>
        /// Prywatna klasa będąca rozszerzeniem klasy MenuItem. Obiekt tej klasy stanowi wyświetlaną listę wyników.
        /// </summary>
        private class ListMenuItem : MenuItem
        {
            /// <summary>
            /// Poziomy padding.
            /// </summary>
            private const int HORIZONTAL_PADDING = 1;
            /// <summary>
            /// Pionowy padding.
            /// </summary>
            private const int VERTICAL_PADDING = 1;

            public ListMenuItem(MenuScene menu, Vector2 pos, HighScores hs)
                : base(menu)
            {
                this.Text = hs.ToString();
                textDim = font.MeasureString(this.Text);

                outer = new Rectangle((int)pos.X, (int)pos.Y, (int)(textDim.X + 2 * HORIZONTAL_PADDING), (int)(textDim.Y + 2 * VERTICAL_PADDING));
                inner = new Rectangle(outer.Left + BORDER, outer.Top + BORDER, outer.Width - 2 * BORDER, outer.Height - 2 * BORDER);

                textNaturalPosition = new Vector2(inner.Left + HORIZONTAL_PADDING, inner.Top + VERTICAL_PADDING);
            }

            public ListMenuItem(MenuScene menu, Rectangle boundaries, HighScores hs)
                : base(menu, boundaries, hs.ToString(), null)
            {
                textNaturalPosition = new Vector2(inner.Left + HORIZONTAL_PADDING, inner.Top + VERTICAL_PADDING);
            }
        }

        /// <summary>
        /// Stała przechowująca napis etykiety tytułowej.
        /// </summary>
        private const string LABEL_BEST_SCORES = "Najlepsi z najlepszych";

        /// <summary>
        /// Etykieta tytułowa.
        /// </summary>
        private MenuItem label;

        /// <summary>
        /// Lista wyników.
        /// </summary>
        private ListMenuItem scoresList;

        /// <summary>
        /// Obiekt przechowujący listę najlepszych wyników.
        /// </summary>
        public HighScores HighScores { get; set; }

        public HighScoreMenu(MinerGame game)
            : this(game, null)
        {

        }

        public HighScoreMenu(MinerGame game, MenuScene previous)
            : base(game, previous)
        {
            this.HighScores = HighScores.GetInstance();
            HighScores.HighScoreList.Add(new Result() { PlayerName = "asd", Score = 10 });
            Rectangle labelRec = new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - MENU_ITEM_WIDTH / 2,
                Game.GraphicsDevice.Viewport.Height / 2 - 300, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT);

            label = new MenuItem(this, labelRec, LABEL_BEST_SCORES, null);

            scoresList = new ListMenuItem(this, new Vector2(label.Boundaries.Left, label.Boundaries.Bottom + 50), this.HighScores);
            scoresList = new ListMenuItem(this, new Rectangle(labelRec.X, labelRec.Y + labelRec.Height + 50, labelRec.Width, labelRec.Height * 2), this.HighScores);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            CurrentKeyState = Keyboard.GetState();
            if (previousMenu != null && CurrentKeyState.IsKeyDown(Keys.Escape) && !OldKeyState.IsKeyDown(Keys.Escape))
            {
                GoBack();
            }
            OldKeyState = CurrentKeyState;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            label.Draw(spriteBatch);
            scoresList.Draw(spriteBatch);
        }
    }
}
