using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca pole typu "Lita skała".
    /// </summary>
    public class SolidRock : Field
    {
        /// <summary>
        /// Nazwa pliku stanowiącego obrazek tego obiektu.
        /// </summary>
        public const string ASSET_NAME = "solidRock";

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private SolidRock() {}

        public SolidRock(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
        }

        /// <summary>
        /// Metoda odpowiadająca za uaktualnienie stanu związanego z obiektem.
        /// </summary>
        /// <param name="gameTime">Czas, jaki upłynął od ostatniego wywołania tej metody.</param>
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
