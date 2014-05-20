using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Scenes
{
    /// <summary>
    /// Klasa reprezentująca przycisk (wraz z krawędzią) w menu.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Ścieżka do czcionki użytej na tej scenie.
        /// </summary>
        private const string FONT_ASSET = "general";

        /// <summary>
        /// Prostokąt stanowiący obwód przycisku.
        /// </summary>
        protected Rectangle outer;
        /// <summary>
        /// Prostokąt stanowiący wnętrze przycisku (bez krawędzi).
        /// </summary>
        protected Rectangle inner;
        /// <summary>
        /// Tekstura, używana do narysowania przycisku.
        /// </summary>
        private Texture2D texture;
        /// <summary>
        /// Czcionka napisów na przycisku.
        /// </summary>
        protected SpriteFont font;

        /// <summary>
        /// Zwraca prostokąt wyznczający granice całego przycisku.
        /// </summary>
        public Rectangle Boundaries
        {
            get
            {
                return outer;
            }
        }

        /// <summary>
        /// Referencja na menu.
        /// </summary>
        protected MenuScene menu;

        /// <summary>
        /// Wymiary tekstu zapisanego czcionką napisów przycisku.
        /// </summary>
        protected Vector2 textDim = Vector2.Zero;

        /// <summary>
        /// Pozycja tekstu na przycisku we współrzędnych okna, którą musi przyjąć tekst, żeby był wyśrodkowany.
        /// </summary>
        protected Vector2 textNaturalPosition = Vector2.Zero;

        /// <summary>
        /// Kolor krawędzi przycisku.
        /// </summary>
        protected readonly Color outerColor = Color.Black;
        /// <summary>
        /// Kolor krawędzi przycisku jeśli jest wybrany.
        /// </summary>
        protected readonly Color outerSelectedColor = Color.BlanchedAlmond;
        /// <summary>
        /// Kolor wnętrza przycisku.
        /// </summary>
        protected readonly Color innerColor = Color.White;
        /// <summary>
        /// Kolor wnętrza przycisku jeśli jest wybrany.
        /// </summary>
        protected readonly Color innerSelectedColor = Color.BlueViolet;

        /// <summary>
        /// Stała określająca grubość krawędzi.
        /// </summary>
        protected const int BORDER = 5;

        /// <summary>
        /// Akcja wykonywana po wciśnięciu przycisku.
        /// </summary>
        private Action<MenuScene> callback;

        /// <summary>
        /// Tekst na przycisku.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Flaga określająca, czy przycisk jest wybrany.
        /// </summary>
        public bool Selected { get; set; }

        public MenuItem(MenuScene menu, Rectangle boundaries, string text, Action<MenuScene> callback)
            : this(menu)
        {
            this.Text = text;
            this.callback = callback;

            textDim = font.MeasureString(this.Text);

            outer = boundaries;
            inner = new Rectangle(boundaries.Left + BORDER, boundaries.Top + BORDER, boundaries.Width - 2 * BORDER, boundaries.Height - 2 * BORDER);

            textNaturalPosition = new Vector2((inner.Left + inner.Right) / 2 - textDim.X / 2, (inner.Top + inner.Bottom) / 2 - textDim.Y / 2);
        }

        public MenuItem(MenuScene menu)
        {
            this.menu = menu;
            texture = new Texture2D(menu.Game.GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });

            font = menu.Game.Content.Load<SpriteFont>(FONT_ASSET);
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, outer, Selected ? outerSelectedColor : outerColor);
            spriteBatch.Draw(texture, inner, Selected ? innerSelectedColor : innerColor);
            spriteBatch.DrawString(font, this.Text, textNaturalPosition, Color.Black);
        }

        /// <summary>
        /// Metoda wywoływana po wciśnięciu przycisku. Uruchamia przekazany wcześniej callback.
        /// </summary>
        public void Action()
        {
            if (callback != null)
            {
                callback(this.menu);
            }
        }
    }
}
