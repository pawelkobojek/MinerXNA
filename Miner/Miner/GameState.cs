using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Miner
{
    /// <summary>
    /// Klasa odpowiedzialna za przetrzymywanie informacji o stanie gry. Plik xml otrzymany z jej serializacji
    /// jest jednocześnie stanem gry.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Aktualnie grający użytkownik.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Aktualny poziom
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        /// Wybrany przez użytkownika poziom trudności.
        /// </summary>
        public DifficultyLevel Difficulty { get; set; }

        /// <summary>
        /// Postać Minera.
        /// </summary>
        public Miner Miner { get; set; }

        /// <summary>
        /// Lista zebranych przez gracza kluczy.
        /// </summary>
        public List<Key> Keys { get; set; }

        /// <summary>
        /// Mapa gry. Przechowuje również informacje o obiektach znajdujących się na polach.
        /// </summary>
        public GameMap Map { get; set; }

        /// <summary>
        /// Lista aktualnie występujących pocisków.
        /// </summary>
        public List<Bullet> Bullets { get; set; }

        /// <summary>
        /// Lista aktualnie występujących przeciwników.
        /// </summary>
        public List<Enemy> Enemies { get; set; }

        /// <summary>
        /// Flaga określająca, czy Kosmiczna Wrota są otwarte (ustawiana wtedy, gdy gracz wykona warunki koniecznie do zakończenia danego etapu).
        /// </summary>
        public bool GatesOpen { get; set; }

        /// <summary>
        /// Flaga określająca, czy dany etap został ukończony, tj. czy gracz wszedł w Kosmiczne Wrota.
        /// </summary>
        public bool LevelCompleted { get; set; }

        public GameState()
        {
            this.User = new User() { Name = "asd" };
            this.Map = new GameMap();
            this.Keys = new List<Key>();
            this.Bullets = new List<Bullet>();
            this.Enemies = new List<Enemy>();
        }
    }
}
