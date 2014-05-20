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
    /// Klasa odpowiedzialna za menu ustawień.
    /// </summary>
    public class SettingsMenu : MenuScene
    {
        /// <summary>
        /// Tekst na etykiecie tytułowej.
        /// </summary>
        private const string LABEL_CONTROLS = "Sterowanie";
        /// <summary>
        /// Tekst na przycisku zmiany wartości przycisku "Góra".
        /// </summary>
        private const string KEY_UP = "Gora";
        /// <summary>
        /// Tekst na przycisku zmiany wartości przycisku "Dół".
        /// </summary>
        private const string KEY_DOWN = "Dol";
        /// <summary>
        /// Tekst na przycisku zmiany wartości przycisku "Lewo".
        /// </summary>
        private const string KEY_LEFT = "Lewo";
        /// <summary>
        /// Tekst na przycisku zmiany wartości przycisku "Prawo".
        /// </summary>
        private const string KEY_RIGHT = "Prawo";
        /// <summary>
        /// Tekst na przycisku zmiany wartości przycisku "Akcja".
        /// </summary>
        private const string KEY_ACTION = "Akcja";

        /// <summary>
        /// Stała określająca domyślną wartość lewego marginesu w tej scenie.
        /// </summary>
        private const int LEFT_MARGIN = 100;

        /// <summary>
        /// Stała określająca domyślną wysokośc przycisku w tej scenie.
        /// </summary>
        private new const int MENU_ITEM_HEIGHT = 75;
        /// <summary>
        /// Stała określająca przerwę między przyciskami w tej scenie.
        /// </summary>
        private new const int GAP = 40;

        /// <summary>
        /// Flaga określająca, czy aktualnie jest zmieniana wartość jakiego przycisku.
        /// </summary>
        private bool changingKey = false;

        /// <summary>
        /// Etykieta tytułowa.
        /// </summary>
        private MenuItem label;

        public SettingsMenu(MinerGame game, MenuScene previous)
            : base(game, previous)
        {
            int start = 50;
            int pos = start;
            label = new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT), LABEL_CONTROLS, null);
            pos += MENU_ITEM_HEIGHT + GAP * 2;
            MenuItems.Add(new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT),
                String.Format("{0}: {1}", KEY_UP, this.Game.Settings.Controls.Up.ToString()), (menu) =>
                    {
                        ChangeKey();
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT),
                String.Format("{0}: {1}", KEY_DOWN, this.Game.Settings.Controls.Down.ToString()), (menu) =>
                    {
                        ChangeKey();
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT),
                String.Format("{0}: {1}", KEY_LEFT, this.Game.Settings.Controls.Left.ToString()), (menu) =>
                    {
                        ChangeKey();
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT),
                String.Format("{0}: {1}", KEY_RIGHT, this.Game.Settings.Controls.Right.ToString()), (menu) =>
                    {
                        ChangeKey();
                    }));
            pos += MENU_ITEM_HEIGHT + GAP;
            MenuItems.Add(new MenuItem(this, new Rectangle(LEFT_MARGIN, pos, MENU_ITEM_WIDTH, MENU_ITEM_HEIGHT),
                String.Format("{0}: {1}", KEY_ACTION, this.Game.Settings.Controls.Action.ToString()), (menu) =>
                    {
                        ChangeKey();
                    }));

            MenuItems[currentlySelected].Selected = true;
        }

        /// <summary>
        /// Metoda wywoływana, gdy użytkownik wyraża wolę zmiany wartości przycisku.
        /// </summary>
        private void ChangeKey()
        {
            changingKey = true;
            MenuItem current = MenuItems[currentlySelected];
            string lastWord = current.Text.Substring(current.Text.LastIndexOf(" ") + 1);
            Console.WriteLine(lastWord);
            current.Text = current.Text.Replace(lastWord, "__");
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            if (changingKey)
            {
                CurrentKeyState = Keyboard.GetState();
                if (!OldKeyState.IsKeyDown(Keys.Enter) && !OldKeyState.IsKeyDown(this.Game.Settings.Controls.Action) &&
                    CurrentKeyState.GetPressedKeys().Length != 0)
                {
                    Keys selectedKey = CurrentKeyState.GetPressedKeys()[0];
                    switch (MenuItems[currentlySelected].Text.Substring(0, MenuItems[currentlySelected].Text.IndexOf(":")))
                    {
                        case KEY_UP:
                            if (ProperKey(selectedKey))
                            {
                                this.Game.Settings.Controls.Up = selectedKey;
                            }
                            else
                            {
                                selectedKey = this.Game.Settings.Controls.Up;
                            }
                            break;
                        case KEY_DOWN:
                            if (ProperKey(selectedKey))
                            {
                                this.Game.Settings.Controls.Down = selectedKey;
                            }
                            else
                            {
                                selectedKey = this.Game.Settings.Controls.Down;
                            }
                            break;
                        case KEY_LEFT:
                            if (ProperKey(selectedKey))
                            {
                                this.Game.Settings.Controls.Left = selectedKey;
                            }
                            else
                            {
                                selectedKey = this.Game.Settings.Controls.Left;
                            }
                            break;
                        case KEY_RIGHT:
                            if (ProperKey(selectedKey))
                            {
                                this.Game.Settings.Controls.Right = selectedKey;
                            }
                            else
                            {
                                selectedKey = this.Game.Settings.Controls.Right;
                            }
                            break;
                        case KEY_ACTION:
                            if (ProperKey(selectedKey))
                            {
                                this.Game.Settings.Controls.Action = selectedKey;
                            }
                            else
                            {
                                selectedKey = this.Game.Settings.Controls.Action;
                            }
                            break;
                        default:
                            break;
                    }
                    MenuItems[currentlySelected].Text = MenuItems[currentlySelected].Text.Replace("__", selectedKey.ToString());

                    changingKey = false;
                }
                OldKeyState = CurrentKeyState;
                return;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Metoda określająca, czy wartość przycisku może być zmieniona na tę, podaną przez użytkownika.
        /// Jest to możliwe tylko wtedy, gdy podany przez przeciwnika klawisz, nie jest już ustawiony na inną akcję.
        /// </summary>
        /// <param name="selectedKey">Wybrany przez przeciwnika klawisz</param>
        /// <returns></returns>
        private bool ProperKey(Keys selectedKey)
        {
            return !(selectedKey == this.Game.Settings.Controls.Up ||
                selectedKey == this.Game.Settings.Controls.Down ||
                selectedKey == this.Game.Settings.Controls.Left ||
                selectedKey == this.Game.Settings.Controls.Right ||
                selectedKey == this.Game.Settings.Controls.Action);
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            label.Draw(spriteBatch);
        }
    }
}
