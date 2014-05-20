using Microsoft.Xna.Framework;
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
    /// Bazowa klasa dla wszystkich przeciwników.
    /// </summary>
    [XmlInclude(typeof(Kosmojopek))]
    [XmlInclude(typeof(Ufolowca))]
    [XmlInclude(typeof(WladcaLaserowejDzidy))]
    [XmlInclude(typeof(PoteznySultan))]
    public abstract class Enemy : GameObject, IMoveable, IMeleeAttacker
    {
        /// <summary>
        /// Wartość impulsu prędkości dodawanego do składowej X prędkości przy wykonaniu ruchu.
        /// </summary>
        protected float speed;

        /// <summary>
        /// Liczba naskoków na głowę do zabicia.
        /// </summary>
        public int JumpOnHitPoints { get; set; }

        /// <summary>
        /// Liczbra trafień laserem do zabicia.
        /// </summary>
        public int LaserHitPoints { get; set; }

        /// <summary>
        /// Liczba punktów doświadczenia uzyskana po zabiciu przeciwnika.
        /// </summary>
        public int ExprienceGained { get; set; }

        /// <summary>
        /// Kierunek ostatniego ruchu, używany do określenia strzału.
        /// </summary>
        public Direction LastMoveDirection { get; set; }

        /// <summary>
        /// Stan obiektu.
        /// </summary>
        protected State state = State.Falling;

        /// <summary>
        /// Grawitacja.
        /// </summary>
        private readonly Vector2 gravity = new Vector2(0, 250f);

        /// <summary>
        /// Konstruktor bezparametrowy.
        /// </summary>
        public Enemy()
        {

        }

        public Enemy(MinerGame game, int posX, int posY, float speed, int jumpOnHitPoints, int laserHitPoints, int expGained, string assetName)
            : this(game, new Vector2(posX * Field.FieldWidth, posY * Field.FieldHeight), speed, jumpOnHitPoints, laserHitPoints, expGained, assetName)
        {
        }

        public Enemy(MinerGame game, Vector2 position, float speed, int jumpOnHitPoints, int laserHitPoints, int expGained, string assetName)
            : base(game, position)
        {
            this.speed = speed;
            this.JumpOnHitPoints = jumpOnHitPoints;
            this.LaserHitPoints = laserHitPoints;
            this.ExprienceGained = expGained;
            this.sprite = game.Content.Load<Texture2D>(assetName);
            this.LastMoveDirection = (new Random().Next() % 2 == 0) ? Direction.left : Direction.right;
            this.Width -= 10.0f;
            this.Height -= 10.0f;
        }

        /// <summary>
        /// Implementacja interfejsu IMoveable. Ruch obiektu w kierunku Direction.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek ruchu</param>
        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    Velocity.X = -speed;
                    LastMoveDirection = Direction.left;
                    break;
                case Direction.right:
                    Velocity.X = speed;
                    LastMoveDirection = Direction.right;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Implementacja interfejsu IMeleeAttacker. Metoda wywoływana przez postać atakującą w zwarciu.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek ataku</param>
        /// <returns>Wartość zadanych obrażeń</returns>
        public int MeleeAttack(Direction direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metoda wywoływana w przypadku naskoku na głowę.
        /// Dekrementuja wartość licznika naskoków na głowę.
        /// Jeśli jego wartość spadnie do zera, przeciwnik zostaje zabity.
        /// </summary>
        public void JumpedOn()
        {

        }

        /// <summary>
        /// Metoda wywoływana po otrzymaniu strzału z lasera.
        /// Dekrementuja wartość licznika trafień laserem.
        /// Jeśli jego wartość spadnie do zera, przeciwnik zostaje zabity.
        /// </summary>
        /// <param name="bullet">Pocisk, którym przeciwnik został trafiony</param>
        public void ShotWithLaser(Bullet bullet)
        {
            game.GameState.Bullets.Remove(bullet);
            LaserHitPoints--;
            if (LaserHitPoints <= 0)
            {
                game.GameState.User.Score += this.ExprienceGained;
                game.GameState.Miner.Experience += this.ExprienceGained;
                game.GameState.Enemies.Remove(this);
            }
        }

        /// <summary>
        /// Implementacja interfejsu IMeleeAttacker. Pole określające wartość obrażeń w zwarciu.
        /// </summary>
        public int MeleeDamage
        {
            get;
            set;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle((int)Position.X, (int)Position.Y, (int)this.Width, (int)this.Height), Color.White);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 oldPosition = this.Position;
            Position += Velocity * time;

            Field topLeft = game.GameState.Map.Fields[(int)Position.X / Field.FieldWidth][(int)Position.Y / Field.FieldHeight];
            Field topRight = game.GameState.Map.Fields[(int)(Position.X + this.Width) / Field.FieldWidth][(int)Position.Y / Field.FieldHeight];
            Field bottomRight = game.GameState.Map.Fields[(int)(Position.X + this.Width) / Field.FieldWidth][(int)(Position.Y + this.Height - 10f) / Field.FieldHeight];
            Field bottomLeft = game.GameState.Map.Fields[(int)Position.X / Field.FieldWidth][(int)(Position.Y + this.Height - 10f) / Field.FieldHeight];

            if ((!topLeft.IsEmpty || !topRight.IsEmpty) || (state != State.OnGround && (!bottomLeft.IsEmpty || !bottomRight.IsEmpty)))
            {
                Position = new Vector2(oldPosition.X, Position.Y);
                LastMoveDirection = (LastMoveDirection == Direction.left) ? Direction.right : Direction.left;
            }

            if (state != State.Digging)
            {
                foreach (var firstDim in this.game.GameState.Map.Fields)
                {
                    foreach (var field in firstDim)
                    {

                        if (!field.IsEmpty && !this.CollidesWith(field))
                        {
                            this.state = State.Falling;
                            break;
                        }
                    }
                }
            }

            if (state != State.OnGround)
            {
                foreach (var firstDim in game.GameState.Map.Fields)
                {
                    foreach (var item in firstDim)
                    {

                        if (!item.IsEmpty && item.Position.Y - this.Position.Y - Field.FieldHeight <= EPS &&
                            this.CollidesWith(item))
                        {
                            state = State.OnGround;
                            Velocity.Y = 0.0f;
                            break;
                        }
                    }

                }

                if (state != State.OnGround)
                {
                    Velocity += gravity * time;
                }
            }
        }
    }
}
