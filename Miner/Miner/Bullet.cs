using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca obiekt pocisku.
    /// </summary>
    public class Bullet : GameObject
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "bullet";

        /// <summary>
        /// Szerokość pocisku.
        /// </summary>
        private int width = Field.FieldWidth / 2;
        /// <summary>
        /// Wysokość pocisku.
        /// </summary>
        private int height = Field.FieldHeight / 8;

        /// <summary>
        /// Wektor prędkości.
        /// </summary>
        private Vector2 velocity = Vector2.Zero;

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private Bullet() { }

        public Bullet(MinerGame game, int posX, int posY, ShootDirection dir)
            : base(game, posX, posY)
        {
            this.Width = width;
            this.Height = height;
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            int one = (dir == ShootDirection.right) ? 1 : -1;
            velocity.X = one * 250.0f;
        }

        public Bullet(MinerGame game, Vector2 position, ShootDirection dir)
            : base(game, position)
        {
            this.Width = width;
            this.Height = height;
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            int one = (dir == ShootDirection.right) ? 1 : -1;
            velocity.X = one * 250.0f;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height), Color.White);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.X + width <= 0 || Position.X >= GameMap.DEF_WIDTH * Field.FieldWidth)
            {
                game.GameState.Bullets.Remove(this);
                return;
            }

            for (int i = 0; i < game.GameState.Enemies.Count; i++)
            {
                if (game.GameState.Enemies[i] != null && this.CollidesWith(game.GameState.Enemies[i]))
                {
                    game.GameState.Enemies[i].ShotWithLaser(this);
                    return;
                }
            }
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
