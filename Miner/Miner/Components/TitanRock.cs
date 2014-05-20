﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca pole typu "Kosmiczne wrota".
    /// </summary>
    public class TitanRock : Field
    {
        /// <summary>
        /// Ścieżka do obrazka reprezentującego obiekt na scenie.
        /// </summary>
        public const string ASSET_NAME = "titanRock";

        /// <summary>
        /// Prywatny konstruktor bezparametrowy, konieczny do procesu serializacji XML.
        /// </summary>
        private TitanRock() { }

        public TitanRock(MinerGame game, int posX, int posY)
            : base(game, posX, posY)
        {
            this.sprite = game.Content.Load<Texture2D>(ASSET_NAME);
        }
    }
}