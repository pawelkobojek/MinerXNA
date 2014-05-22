using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca zwykłe pole. Jednocześnie stanowi klasę bazową dla pól specjalnych.
    /// </summary>
    [XmlInclude(typeof(TitanRock))]
    [XmlInclude(typeof(SpaceGate))]
    [XmlInclude(typeof(CosmicMatter))]
    [XmlInclude(typeof(SolidRock))]
    public class Field : GameObject
    {
        /// <summary>
        /// Szerokość pola w pikselach.
        /// </summary>
        public static int FieldWidth = 10;

        /// <summary>
        /// Wysokość pola w pikselach.
        /// </summary>
        public static int FieldHeight = 10;

        /// <summary>
        /// Czas w milisekundach, w którym pole pozostaje okdryte.
        /// </summary>
        public const int REVEAL_TIME = 3000;

        /// <summary>
        /// Czas milisekundach, w którym pole pozostaje odkopane.
        /// </summary>
        public const int DIG_TIME = 3000;

        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "ground";

        /// <summary>
        /// Ilośc punktów za wykopanie pola
        /// </summary>
        public const int SCORE_FOR_DIG = 5;

        /// <summary>
        /// Flaga mówiąca, czy pole jest odkryte.
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Flaga mówiąca, czy pole jest puste.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Flaga mówiąca, czy pole jest wykopane.
        /// </summary>
        public bool IsDigged { get; set; }

        /// <summary>
        /// Referencja na bonus znajdujący się na tym polu (null, jeśli go nie ma).
        /// </summary>
        public BonusItem Bonus { get; set; }

        /// <summary>
        /// Czas w milisekundach który upłynął od momentu odsłonięcia pola.
        /// Przestaje być liczony gdy pole znowu zostanie zasłonięte.
        /// </summary>
        private int revealTime;
        /// <summary>
        /// Czas w milisekundach który upłynął od momentu wykopania pola.
        /// Przestaje być liczony gdy pole znowu zostanie zakopane.
        /// </summary>
        private int digTime;

        public Field()
            : base()
        {
            Bonus = null;
        }

        public Field(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.Position = new Vector2(posX * FieldWidth, posY * FieldHeight);
            this.IsRevealed = false;
            this.IsEmpty = false;
            this.IsDigged = false;
        }

        public Field(MinerGame game, int posX, int posY, bool isEmpty)
            : this(game, posX, posY)
        {
            this.IsEmpty = isEmpty;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Bonus != null)
            {
                Bonus.Draw(spriteBatch);
            }

            if (!IsEmpty && !IsRevealed)
            {
                spriteBatch.Draw(sprite, new Rectangle(FieldWidth * PosX, FieldHeight * PosY, FieldWidth, FieldHeight), Color.White);
            }
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsRevealed)
            {
                revealTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (revealTime <= 0)
                {
                    IsRevealed = false;
                }
            }
            if (IsDigged)
            {
                digTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (digTime <= 0)
                {
                    for (int i = 0; i < game.GameState.Enemies.Count; i++)
                    {
                        if (game.GameState.Enemies[i].CollidesWith(this))
                        {
                            game.GameState.Enemies.Remove(game.GameState.Enemies[i]);
                        }
                        if (game.GameState.Miner.CollidesWith(this))
                        {
                            game.ExitGame();
                        }
                    }
                    IsDigged = false;
                    IsEmpty = false;
                }
            }
        }

        /// <summary>
        /// Metoda odkrywa pole i rozpoczyna odliczanie czasu jego odkrycia.
        /// </summary>
        public void Reveal()
        {
            this.IsRevealed = true;
            this.revealTime = REVEAL_TIME;
        }

        /// <summary>
        /// Metoda odkopuje pole i rozpoczyna odliczanie czasu jego odkrycia.
        /// </summary>
        public void Dig()
        {
            this.IsDigged = true;
            this.IsEmpty = true;
            this.digTime = DIG_TIME;
        }

        /// <summary>
        /// Zwraca ścieżkę do obrazka. Zawsze zwraca ASSET_NAME.
        /// </summary>
        /// <returns>Ścieżkę do obrazka.</returns>
        public override string GetAssetName()
        {
            return ASSET_NAME;
        }
    }
}
