using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa bazowa dla wszystkich widzialnych obiektów w grze.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Stała stanowiąca dokładność używana w porównaniach między liczbami zmiennoprzeciwnkowymi.
        /// </summary>
        protected const float EPS = 0.0001f;

        /// <summary>
        /// Pozycja X wskazująca pole, na którym znajduje się obiekt.
        /// </summary>
        public int PosX { get; set; }

        /// <summary>
        /// Pozycja Y wskazująca pole, na którym znajduje się obiekt.
        /// </summary>
        public int PosY { get; set; }

        /// <summary>
        /// Szerokość obiektu w pikselach.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Wysokość obiektu w pikselach.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Referenca na obiekt gry.
        /// </summary>
        [XmlIgnore]
        public MinerGame game = null;

        /// <summary>
        /// Dokładna pozycja obiektu na ekranie.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Prędkość obiektu.
        /// </summary>
        protected Vector2 Velocity;

        /// <summary>
        /// Referencja na obrazek reprezentujący obiekt.
        /// </summary>
        public Texture2D sprite;

        /// <summary>
        /// Konstruktor bezparametrowy.
        /// </summary>
        public GameObject()
        {
            this.Velocity = Vector2.Zero;
            this.Width = Field.FieldWidth;
            this.Height = Field.FieldHeight;
        }

        public GameObject(MinerGame game)
            : this()
        {
            this.game = game;
        }

        public GameObject(MinerGame game, int posX, int posY)
            : this(game)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.Position = new Vector2(posX * Field.FieldWidth, posY * Field.FieldHeight);
        }

        public GameObject(MinerGame game, Vector2 position)
            : this(game)
        {
            this.Position = position;
        }

        /// <summary>
        /// Metoda odpowiadająca za renderowanie obiektu na ekranie.
        /// </summary>
        /// <param name="spriteBatch">Referencja na SpriteBatch, na którym obiekt będzie rysowany.</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Metoda sprawdzająca, czy dany obiekt koliduje (w dowolnym kierunku) z innym.
        /// </summary>
        /// <param name="obj">Obiekt, z który mają być sprawdzane kolizje.</param>
        /// <returns></returns>
        public bool CollidesWith(GameObject obj)
        {
            //   return intersects(obj);
            //return ((this.Position.Y <= obj.Position.Y + Field.FieldHeight && this.Position.Y + Field.FieldHeight >= obj.Position.Y) ||
            //    (this.Position.Y + Field.FieldHeight >= obj.Position.Y && this.Position.Y + Field.FieldHeight <= obj.Position.Y + Field.FieldHeight))
            //    &&
            //    ((this.Position.X <= obj.Position.X + Field.FieldWidth && this.Position.X >= obj.Position.X) ||
            //    (this.Position.X + Field.FieldWidth <= obj.Position.X + Field.FieldWidth && this.Position.X + Field.FieldWidth >= obj.Position.X));
            bool collided = ((this.Position.Y <= obj.Position.Y + obj.Height && this.Position.Y >= obj.Position.Y) ||
                (this.Position.Y + this.Height >= obj.Position.Y && this.Position.Y + this.Height <= obj.Position.Y + obj.Height))
                &&
                ((this.Position.X <= obj.Position.X + obj.Width && this.Position.X >= obj.Position.X) ||
                (this.Position.X + this.Width <= obj.Position.X + obj.Width && this.Position.X + this.Width >= obj.Position.X));
            if (collided && game.GameState.GatesOpen && obj is SpaceGate)
            {
                game.GameState.LevelCompleted = true;
            }

            return collided;
        }

        /// <summary>
        /// Metoda sprawdzająca, czy dany obiekt koliduje z innym, ale tylko w kiernku poziomym.
        /// </summary>
        /// <param name="obj">Obiekt, z który mają być sprawdzane kolizje.</param>
        /// <returns></returns>
        public bool CollidesHorizontallyWith(GameObject obj)
        {
            //return ((this.Position.Y <= obj.Position.Y + Field.FieldHeight && this.Position.Y + Field.FieldHeight >= obj.Position.Y) ||
            //    (this.Position.Y + Field.FieldHeight >= obj.Position.Y && this.Position.Y + Field.FieldHeight <= obj.Position.Y + Field.FieldHeight))
            //    &&
            //    ((this.Position.X <= obj.Position.X + Field.FieldWidth && this.Position.X >= obj.Position.X) ||
            //    (this.Position.X + Field.FieldWidth <= obj.Position.X + Field.FieldWidth && this.Position.X + Field.FieldWidth >= obj.Position.X));
            //return ((this.Position.X <= obj.Position.X + obj.Width && this.Position.X >= obj.Position.X) ||
            //(this.Position.X + this.Width <= obj.Position.X + obj.Width && this.Position.X + this.Width >= obj.Position.X));
            return ((this.Position.Y + this.Height < obj.Position.Y + obj.Height) && (this.Position.Y + this.Height > obj.Position.Y) &&
                ((Math.Abs(this.Position.X + this.Width - obj.Position.X) < EPS)));
        }

        //public bool intersects(GameObject r)
        //{
        //    float tw = this.Width;
        //    float th = this.Height;
        //    float rw = r.Width;
        //    float rh = r.Height;
        //    if (rw <= 0 || rh <= 0 || tw <= 0 || th <= 0)
        //    {
        //        return false;
        //    }
        //    float tx = this.Position.X;
        //    float ty = this.Position.Y;
        //    float rx = r.Position.X;
        //    float ry = r.Position.Y;
        //    rw += rx;
        //    rh += ry;
        //    tw += tx;
        //    th += ty;
        //    //      overflow || intersect
        //    return ((rw < rx || rw > tx) &&
        //            (rh < ry || rh > ty) &&
        //            (tw < tx || tw > rx) &&
        //            (th < ty || th > ry));
        //}
    }
}
