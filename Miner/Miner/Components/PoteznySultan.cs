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
    /// Klasa reprezentująca przeciwnika "Potężny Sułtan" - najpotężniejszego i ostatniego przeciwnika.
    /// </summary>
    public class PoteznySultan : Enemy, IJumpable, IRangeAttacker
    {
        /// <summary>
        /// Stała będąca domyślną wartością impulsu skoku.
        /// </summary>
        private const float JUMP_SPEED = -230.0f;

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private PoteznySultan() { }

        /// <summary>
        /// Implementacja interfejsu IRangeAttacker. Metoda wywoływana przez postać atakującą na odległość.
        /// </summary>
        /// <param name="direction">Wyliczenie określające kierunek strzału</param>
        /// <param name="damage">Wartość zadanych obrażeń</param>
        /// <returns>Obiekt pocisku stworzony w wyniku strzału</returns>
        public Bullet RangeAttack(ShootDirection direction, int damage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementacja interfejsu IJumpeable. Metoda odpowiadająca za wykonanie przez postać skoku poprzez nadanie pionowego impulsu o wartości równej JumpImpulse.
        /// </summary>
        public void Jump()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementacja interfejsu IJumpeable. Pole określające wartość pionowo skierowanego impulsu nadanego w momencie skoku.
        /// </summary>
        public float JumpImpulse
        {
            get;
            set;
        }

        public int RangedDamage
        {
            get;
            set;
        }
    }
}
