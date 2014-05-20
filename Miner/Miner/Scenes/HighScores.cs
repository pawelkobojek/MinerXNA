using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    /// <summary>
    /// Klasa przechowująca informacje o najlepszych wynikach. Jest to singleton.
    /// </summary>
    public class HighScores
    {
        /// <summary>
        /// Referencja na instancję singletona.
        /// </summary>
        private static HighScores instance = new HighScores();

        /// <summary>
        /// Lista przechowująca najlepsze wyniki. Implementacja klasy zapewnia, że są one posortowane
        /// malejąco, oraz jest ich najwyżej 10.
        /// </summary>
        public List<Result> HighScoreList { get; private set; }

        private HighScores()
        {
            HighScoreList = new List<Result>(10);
        }

        /// <summary>
        /// Dodaje wynik do listy, zapewniając porządek malejący i maksymalną ilość.
        /// </summary>
        /// <param name="result">Wstawiany wynik</param>
        /// <returns>Indeks porządkowy wstawionego wyniku</returns>
        public int AddResult(Result result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Przeciążenie metody ToString(). 
        /// </summary>
        /// <returns>Łańcuch zawierający listę wyników wraz z liczbą porządkową oraz oddzielony znakami nowej linii.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < HighScoreList.Count; ++i)
            {
                sb.Append(String.Format("{0}. {1}       {2}\n", i + 1, HighScoreList[i].PlayerName, HighScoreList[i].Score));
            }
            return sb.ToString();
        }


        /// <summary>
        /// Zwraca, lub tworzy nową instancję singletona.
        /// </summary>
        /// <returns>Instacja singletona.</returns>
        public static HighScores GetInstance()
        {
            if (instance == null)
            {
                instance = new HighScores();
            }

            return instance;
        }
    }
}
