using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca przecwnika "Ufołowca".
    /// </summary>
    public class Ufolowca : Enemy, IJumpable
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "Enemies/ufolowca";

        /// <summary>
        /// Stała będąca domyślną wartością impulsu skoku.
        /// </summary>
        private const float JUMP_SPEED = -230.0f;

        /// <summary>
        /// Stała będąca domyślną wartością składowej X wektora prędkości.
        /// </summary>
        private static readonly float SPEED = Field.FieldWidth;
        /// <summary>
        /// Stała będąca domyślną ilością naskoków do zabicia.
        /// </summary>
        private const int JUMP_ON_HITS = 2;
        /// <summary>
        /// Stała będąca domyślną liczbą trafien laserem do zabicia.
        /// </summary>
        private const int LASER_HITS = 1;
        /// <summary>
        /// Stała będąca domyślną ilością otrzymanego doświadczenia.
        /// </summary>
        private const int EXP_GAINED = 10;
        /// <summary>
        /// Maksymalny czas (w milisekundach) po którym Kosmojopek zmieni kierunek ruchu.
        /// </summary>
        private const int MAX_MOVE_SWITCH_TIME = 20000;

        private Random rand = new Random();
        /// <summary>
        /// Licznik milisekund od ostatniej zmiany kierunku ruchu.
        /// </summary>
        private int timeToSwitch;

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private Ufolowca() { }

        public Ufolowca(MinerGame game, int posX, int posY)
            : base(game, posX, posY, SPEED, JUMP_ON_HITS, LASER_HITS, EXP_GAINED, ASSET_NAME)
        {
            timeToSwitch = rand.Next(MAX_MOVE_SWITCH_TIME);
            JumpImpulse = JUMP_SPEED;
        }

        public Ufolowca(MinerGame game, Vector2 position)
            : base(game, position, SPEED, JUMP_ON_HITS, LASER_HITS, EXP_GAINED, ASSET_NAME)
        {
            timeToSwitch = rand.Next(MAX_MOVE_SWITCH_TIME);
            JumpImpulse = JUMP_SPEED;
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Move(LastMoveDirection);
            timeToSwitch -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeToSwitch <= 0)
            {
                timeToSwitch = rand.Next(MAX_MOVE_SWITCH_TIME);
                LastMoveDirection = (LastMoveDirection == Direction.right) ? Direction.left : Direction.right;
            }

            if (state == State.OnGround)
                Jump();
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
        /// Implementacja interfejsu IJumpeable. Metoda odpowiadająca za wykonanie przez postać skoku poprzez nadanie pionowego impulsu o wartości równej JumpImpulse.
        /// </summary>
        public void Jump()
        {
            state = State.Jumping;
            Velocity.Y = JumpImpulse;
        }
    }
}
