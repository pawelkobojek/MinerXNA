using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla wszystkich scen.
    /// </summary>
    public abstract class MenuScene
    {
        /// <summary>
        /// Zmienna przechowująca stan klawiatury po ostatnim wywołaniu metody Update. Używana, żeby zapobiec wielokrotnemu odczytaniu jednego przyciśnięcia.
        /// </summary>
        public KeyboardState OldKeyState { get; set; }
        /// <summary>
        /// Zmienna przechowująca aktualny stan klawiatury. Potrzebna przy przejściach między poszczególnymi scenami, żeby przekazać jej wartość 
        /// jako OldKeyState nowego menu.
        /// </summary>
        public KeyboardState CurrentKeyState { get; set; }

        /// <summary>
        /// Stała określająca domyślną szerokość przycisku w każdej scenie..
        /// </summary>
        protected const int MENU_ITEM_WIDTH = 400;
        /// <summary>
        /// Stała określająca domyślną wysokośc przycisku w każdej scenie.
        /// </summary>
        protected const int MENU_ITEM_HEIGHT = 100;
        /// <summary>
        /// Stała określająca przerwę między przyciskami w każdej scenie.
        /// </summary>
        protected const int GAP = 50;

        /// <summary>
        /// Referencja na obiekt gry.
        /// </summary>
        public MinerGame Game { get; protected set; }

        /// <summary>
        /// Flaga określająca, czy muzyka jest włączona czy nie.
        /// Opcja wspólna dla wszystkich scen.
        /// </summary>
        public bool SoundOn { get; set; }
        /// <summary>
        /// Flaga określająca, czy muzyka jest włączona czy nie.
        /// Opcja wspólna dla wszystkich scen.
        /// </summary>
        public bool MusicOn { get; set; }

        /// <summary>
        /// Lista przycisków.
        /// </summary>
        public List<MenuItem> MenuItems { get; set; }

        /// <summary>
        /// Indeks aktualnie wybranego przycisku.
        /// </summary>
        protected int currentlySelected = 0;

        /// <summary>
        /// Referencja na poprzednie menu. Używana, żeby zapewnić możliwość powrotu.
        /// </summary>
        protected MenuScene previousMenu;

        public MenuScene(MinerGame game)
        {
            this.Game = game;
            MenuItems = new List<MenuItem>();
        }

        public MenuScene(MinerGame game, MenuScene previous)
            : this(game)
        {
            previousMenu = previous;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in MenuItems)
            {
                item.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public virtual void Update(GameTime gameTime)
        {
            CurrentKeyState = Keyboard.GetState();

            if (CurrentKeyState.IsKeyDown(this.Game.Settings.Controls.Down) && !OldKeyState.IsKeyDown(this.Game.Settings.Controls.Down) ||
                CurrentKeyState.IsKeyDown(Keys.Down) && !OldKeyState.IsKeyDown(Keys.Down))
            {
                MenuItems[currentlySelected].Selected = false;

                currentlySelected = (currentlySelected + 1 >= MenuItems.Count) ? 0 : currentlySelected + 1;

                MenuItems[currentlySelected].Selected = true;
            }
            else if (CurrentKeyState.IsKeyDown(this.Game.Settings.Controls.Up) && !OldKeyState.IsKeyDown(this.Game.Settings.Controls.Up) ||
                CurrentKeyState.IsKeyDown(Keys.Up) && !OldKeyState.IsKeyDown(Keys.Up))
            {
                MenuItems[currentlySelected].Selected = false;

                currentlySelected = (currentlySelected - 1 < 0) ? MenuItems.Count - 1 : currentlySelected - 1;

                MenuItems[currentlySelected].Selected = true;
            }
            else if ((CurrentKeyState.IsKeyDown(Keys.Enter) && !OldKeyState.IsKeyDown(Keys.Enter)) ||
                (CurrentKeyState.IsKeyDown(this.Game.Settings.Controls.Action) && !OldKeyState.IsKeyDown(this.Game.Settings.Controls.Action)))
            {
                MenuItems[currentlySelected].Action();
            }
            else if (previousMenu != null && CurrentKeyState.IsKeyDown(Keys.Escape) && !OldKeyState.IsKeyDown(Keys.Escape))
            {
                GoBack();
            }

            OldKeyState = CurrentKeyState;
        }

        /// <summary>
        /// Metoda wywoływana, żeby wrócić do poprzedniego menu.
        /// </summary>
        protected void GoBack()
        {
            previousMenu.OldKeyState = this.CurrentKeyState;
            MenuManager.GetInstance().CurrentMenu = previousMenu;
        }
    }
}
