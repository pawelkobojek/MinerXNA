using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca obiekt Minera - postaci sterowanej przez gracza.
    /// </summary>
    public class Miner : GameObject, IMoveable, IJumpable, IRangeAttacker, IDiggable
    {
        /// <summary>
        /// Stała określająca zużycie paliwa w jednostkach na ruch.
        /// </summary>
        public const int FUEL_PER_MOVE_EXPENDITURE = 1;

        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "miner";

        /// <summary>
        /// Zmienna określająca stan postaci.
        /// </summary>
        private State state = State.Falling;

        /// <summary>
        /// Kierunek ostatniego ruchu, używany do określenia strzału.
        /// </summary>
        public ShootDirection LastMoveDirection { get; set; }

        /// <summary>
        /// Referencja na aktualny bonus (null jeśli go nie ma).
        /// </summary>
        private BonusItem currentBonus = null;

        //private Vector2 fallingSpeed = new Vector2(0.0f, 10.0f);
        /// <summary>
        /// Zmienna przechowująca pozycje postaci w momencie rozpoczęcia kopania.
        /// </summary>
        private Vector2 digStartPosition = Vector2.Zero;

        /// <summary>
        /// Referencja na obiekt skafandra.
        /// </summary>
        public Nanosuit Suit { get; set; }

        /// <summary>
        /// Zmienna tylko do odczytu stanowiąca wektor przyśpieszenia grawitacji.
        /// </summary>
        private readonly Vector2 gravity = new Vector2(0, 250f);

        /// <summary>
        /// Zmienna przechowująca stan klawiatury po ostatnim wywołaniu metody Update. Używana, żeby zapobiec wielokrotnemu odczytaniu jednego przyciśnięcia.
        /// </summary>
        private KeyboardState OldKeyState;

        /// <summary>
        /// Wartość impulsu prędkości dodawanego do składowej X prędkości przy wykonaniu ruchu.
        /// </summary>
        private float speed = 100.0f;

        /// <summary>
        /// Stała będąca domyślną wartością impulsu skoku.
        /// </summary>
        private const float JUMP_SPEED = -250.0f;

        /// <summary>
        /// Liczba punktów doświadczenia.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Liczba punktów doświadczenia potrzebna do osiągnięcia następnego poziomu.
        /// </summary>
        public int ExpToNextLevel { get; set; }

        /// <summary>
        /// Przebyta droga. Pole używane do obliczenia wydatkowania paliwa. Zerowane gdy osiągnie szerokość pola.
        /// </summary>
        private float distanceTraveled = 0.0f;

        //public int nanoSuitLevel = 0;

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private Miner() { }

        public Miner(MinerGame game)
            : base(game)
        {
            this.Suit = new Nanosuit();
            this.ExpToNextLevel = 100;
            this.Width -= 10.0f;
            this.Height -= 10.0f;
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
            this.JumpImpulse = JUMP_SPEED;
            LastMoveDirection = ShootDirection.right;
            this.currentBonus = new Laser(this.game);
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(this.sprite, new Rectangle(PosX * Field.FieldWidth, PosY * Field.FieldHeight, Field.FieldWidth, Field.FieldHeight), Color.White);
            spriteBatch.Draw(this.sprite, new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height), Color.White);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyState = Keyboard.GetState();

            if (state == State.Digging)
            {
                Position += Velocity * time;
                if (Position.Y > (digStartPosition.Y + Field.FieldHeight))
                {
                    Velocity.Y = 0.0f;
                    state = State.OnGround;
                }
                return;
            }
            // PrintState();

            if (state != State.OnGround && state != State.Digging)
            {
                foreach (var firstDim in game.GameState.Map.Fields)
                {
                    foreach (var item in firstDim)
                    {
                        if (!item.IsEmpty && item.Position.Y - this.Position.Y - Field.FieldHeight <= EPS &&
                            this.CollidesWith(item))
                        {
                            state = State.OnGround;
                            //this.Position = new Vector2(this.Position.X, item.Position.Y - this.Height);
                            Velocity.Y = 0.0f;
                            DoubleJump dJump = currentBonus as DoubleJump;
                            if (dJump != null)
                            {
                                dJump.Used = false;
                            }
                            break;
                        }
                    }
                }

                if (state != State.OnGround)
                {
                    Velocity += gravity * time;
                }
            }

            if (keyState.IsKeyDown(this.game.Settings.Controls.Up) && !OldKeyState.IsKeyDown(this.game.Settings.Controls.Up) &&
                (state == State.OnGround || (state == State.Jumping && currentBonus is DoubleJump && !((DoubleJump)currentBonus).Used)))
            {
                Jump();
            }

            if (keyState.IsKeyDown(this.game.Settings.Controls.Left))
            {
                Move(Direction.left);
            }
            else if (keyState.IsKeyDown(this.game.Settings.Controls.Right))
            {
                Move(Direction.right);
            }
            else if (keyState.IsKeyDown(this.game.Settings.Controls.Down) && !OldKeyState.IsKeyDown(this.game.Settings.Controls.Down) && state == State.OnGround)
            {
                Dig();
            }
            else if (state != State.Jumping)
            {
                Velocity.X = 0.0f;
            }

            Vector2 oldPosition = this.Position;
            Position += Velocity * time;

            distanceTraveled += Math.Abs(Position.X - oldPosition.X);
            if (distanceTraveled >= Field.FieldWidth)
            {
                distanceTraveled = 0.0f;
                this.Suit.Fuel -= FUEL_PER_MOVE_EXPENDITURE;
                this.game.GameState.User.Score += FUEL_PER_MOVE_EXPENDITURE;

                if (Suit.Fuel <= 0)
                {
                    this.game.ExitGame();
                }
            }

            int topLeftX = (int)Position.X / Field.FieldWidth;
            int topLeftY = (int)Position.Y / Field.FieldHeight;
            int topRightX = (int)(Position.X + this.Width) / Field.FieldWidth;
            int topRightY = (int)Position.Y / Field.FieldHeight;
            int bottomLeftX = (int)Position.X / Field.FieldWidth;
            int bottomLeftY = (int)(Position.Y + this.Height - 10f) / Field.FieldHeight;
            int bottomRightX = (int)(Position.X + this.Width) / Field.FieldWidth;
            int bottomRightY = (int)(Position.Y + this.Height - 10f) / Field.FieldHeight;

            Field topLeft = (topLeftX >= 0 && topLeftY >= 0 && topLeftX < game.GameState.Map.Fields.Length && topLeftY < game.GameState.Map.Fields[0].Length) ?
                game.GameState.Map.Fields[topLeftX][topLeftY] : null;
            Field topRight = (topRightX >= 0 && topRightY >= 0 && topRightX < game.GameState.Map.Fields.GetLength(0) && topRightY < game.GameState.Map.Fields[0].Length) ?
                game.GameState.Map.Fields[topRightX][topRightY] : null;
            Field bottomRight = (bottomRightX >= 0 && bottomRightY >= 0 && bottomRightX < game.GameState.Map.Fields.GetLength(0) && bottomRightY < game.GameState.Map.Fields[0].Length) ?
                game.GameState.Map.Fields[bottomRightX][bottomRightY] : null;
            Field bottomLeft = (bottomLeftX >= 0 && bottomLeftY >= 0 && bottomLeftX < game.GameState.Map.Fields.GetLength(0) && bottomLeftY < game.GameState.Map.Fields[0].Length) ?
                game.GameState.Map.Fields[bottomLeftX][bottomLeftY] : null;

            if (((topLeft == null || topRight == null || bottomLeft == null || bottomRight == null) ||
                (!topLeft.IsEmpty || !topRight.IsEmpty)) ||
                (state != State.OnGround && (!bottomLeft.IsEmpty || !bottomRight.IsEmpty)))
            {
                if (oldPosition.X < 0)
                {
                    oldPosition.X = 0;
                }
                if (oldPosition.X >= Field.FieldWidth * GameMap.DEF_WIDTH)
                {
                    oldPosition.X = Field.FieldWidth * GameMap.DEF_WIDTH - this.Width;
                }
                Position = new Vector2(oldPosition.X, Position.Y);
            }

            bool shouldFall = true;
            if (state != State.Digging && state != State.Jumping)
            {
                foreach (var firstDim in this.game.GameState.Map.Fields)
                {
                    foreach (var field in firstDim)
                    {

                        if (!field.IsEmpty && this.CollidesWith(field))
                        {
                            shouldFall = false;
                            break;
                        }
                    }

                }
                if (shouldFall)
                {
                    this.state = State.Falling;
                }
            }

            foreach (var firstDim in game.GameState.Map.Fields)
            {
                foreach (var item in firstDim)
                {

                    if (item.IsEmpty && item.Bonus != null)
                    {
                        if (this.CollidesWith(item))
                        {
                            Fuel f = item.Bonus as Fuel;
                            if (f != null)
                            {
                                Suit.AddFuel(f);
                            }
                            else if (item.Bonus is Key)
                            {
                                game.GameState.Keys.Add(item.Bonus as Key);
                                // TODO
                                if (game.GameState.Keys.Count == game.GameState.CurrentLevel + 2)
                                {
                                    game.GameState.GatesOpen = true;
                                }
                            }
                            else
                            {
                                currentBonus = item.Bonus;
                                //Console.WriteLine("CurrentBonus: {0}", currentBonus.ToString());
                            }
                            item.Bonus = null;
                            break;
                        }
                    }
                }
            }

            if (keyState.IsKeyDown(this.game.Settings.Controls.Action) && !OldKeyState.IsKeyDown(this.game.Settings.Controls.Action))
            {
                IActionUsable actionBonus = currentBonus as IActionUsable;
                if (actionBonus != null)
                {
                    actionBonus.UseAction();
                }
            }

            if (currentBonus != null)
            {
                currentBonus.Update(gameTime);
                currentBonus.RemainingUsageTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (currentBonus.RemainingUsageTime <= 0)
                {
                    //Console.WriteLine("CurrentBonus destroyed");
                    currentBonus = null;
                }
            }

            // Check whether Miner has enough experience to level up
            if (this.Experience >= this.ExpToNextLevel)
            {
                // Excessed experience points pass to the next level.
                this.Experience = this.Experience - this.ExpToNextLevel;
                this.ExpToNextLevel *= 2;
                this.Suit.IncreaseNanosuitLevel();
            }

            OldKeyState = keyState;
        }

        /// <summary>
        /// Metoda pomocnicza, która wypisuje stan na konsolę.
        /// </summary>
        private void PrintState()
        {
            switch (state)
            {
                case State.OnGround:
                    Console.WriteLine("OnGround");
                    break;
                case State.Jumping:
                    Console.WriteLine("Jumping");
                    break;
                case State.Falling:
                    Console.WriteLine("Falling");
                    break;
                case State.Digging:
                    Console.WriteLine("Digging");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Implementacja interfejsu IJumpeable. Metoda odpowiadająca za wykonanie przez postać skoku poprzez nadanie pionowego impulsu o wartości równej JumpImpulse.
        /// </summary>
        public void Jump()
        {
            //this.Position += -fallingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (Position.Y <= JumpStartedPosition.Y - JumpHeight)
            //{
            //    // Achieved maximum height
            //    IsJumping = false;
            //    Velocity.Y = 0.0f;
            //    return;
            //}

            //Velocity -= gravity * time;
            //Console.WriteLine("State: {0}", state == State.Jumping ? "Jumping" : "OnGround");
            DoubleJump dJump = currentBonus as DoubleJump;
            if (dJump != null)
            {
                if (state == State.Jumping)
                {
                    dJump.Used = true;
                }
            }
            state = State.Jumping;
            Velocity.Y = JumpImpulse;
        }

        /// <summary>
        /// Implementacja interfejsu IMoveable. Ruch obiektu w kierunku Direction.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek ruchu</param>
        public void Move(Direction direction)
        {
            //foreach (var item in game.GameState.Map.Fields)
            //{
            //    if (!item.IsEmpty && this.CollidesHorizontallyWith(item))
            //    {
            //        Velocity.X = 0.0f;
            //        Console.WriteLine("({0}, {1})", item.PosX, item.PosY);
            //        return;
            //    }

            //}

            int posX = (int)(this.Position.X / Field.FieldWidth);
            int posY = (int)(this.Position.Y / Field.FieldHeight);
            int posBY = (int)((this.Position.Y + 1f + this.Height) / Field.FieldHeight);
            //Console.WriteLine("({0}, {1})", posX, posY);

            switch (direction)
            {
                case Direction.left:
                    //posX = (int)((this.Position.X + this.Width) / Field.FieldWidth);
                    //posY = (int)(this.Position.Y / Field.FieldHeight);
                    //posBY = (int)((this.Position.Y + this.Height - 1f) / Field.FieldHeight);
                    //if (posX - 1 > 0 && game.GameState.Map.Fields[posX - 1, posY].IsEmpty && game.GameState.Map.Fields[posX - 1, posBY].IsEmpty)
                    //{
                    Velocity.X = -speed;
                    //}
                    //else
                    //{
                    //    Velocity.X = 0f;
                    //}
                    LastMoveDirection = ShootDirection.left;
                    break;
                case Direction.right:
                    //if (posX + 1 < game.GameState.Map.Fields.Length && game.GameState.Map.Fields[posX + 1, posY].IsEmpty && game.GameState.Map.Fields[posX + 1, posBY].IsEmpty)
                    //{
                    Velocity.X = speed;
                    //}
                    //else
                    //{
                    //    Velocity.X = 0f;
                    //}
                    LastMoveDirection = ShootDirection.right;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Implementacja interfejsu IRangeAttacker. Metoda wywoływana przez postać atakującą na odległość.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek strzału</param>
        /// <param name="damage">Wartość zadanych obrażeń</param>
        /// <returns>Obiekt pocisku stworzony w wyniku strzału</returns>
        public Bullet RangeAttack(ShootDirection direction, int damage)
        {
            return new Bullet(this.game, new Vector2(this.Position.X, this.Position.Y + 0.5f * (float)Field.FieldHeight), direction);
        }

        /// <summary>
        /// Implementacja interfejsu IDiggable.
        /// </summary>
        public void Dig()
        {
            float horizontalCenter = (this.Position.X + this.Position.X + this.Width) / 2;

            // find nearest field
            Field diggedField = null;
            foreach (var firstDim in this.game.GameState.Map.Fields)
            {
                foreach (var field in firstDim)
                {

                    if (!field.IsEmpty && !(field is TitanRock)/* && Math.Abs(field.Position.Y - (this.Position.Y + Field.FieldHeight)) <= EPS*/ &&
                        horizontalCenter <= field.Position.X + this.Width && horizontalCenter >= field.Position.X)
                    {
                        diggedField = field;
                        //Console.WriteLine("Dig() {0}, {1}", field.PosX, field.PosY);
                        break;
                    }
                }

            }
            if (diggedField != null)
            {
                this.Position = new Vector2(diggedField.Position.X + 5.0f, this.Position.Y);
                this.digStartPosition = this.Position;
                diggedField.Dig();
                this.game.GameState.User.Score += Field.SCORE_FOR_DIG;
                this.state = State.Digging;
                Velocity.Y = Field.FieldHeight / 1;
            }
        }

        /// <summary>
        /// Implementacja interfejsu IRangedAttacker
        /// </summary>
        public int RangedDamage
        {
            get;
            set;
        }

        /// <summary>
        /// Implementacja interfejsu IJumpeable. Pole określające wartość pionowo skierowanego impulsu nadanego w momencie skoku.
        /// </summary>
        public float JumpImpulse
        {
            get;
            set;
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
