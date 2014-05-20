using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    /// <summary>
    /// Klasa reprezentująca skafander Minera.
    /// </summary>
    public class Nanosuit
    {
        /// <summary>
        /// Ilość paliwa.
        /// </summary>
        private int fuel = 100;

        /// <summary>
        /// Wytrzymałość skafandra.
        /// Jeśli spadnie do zera, gra kończy się porażką gracza.
        /// </summary>
        private int durability = 100;

        /// <summary>
        /// Ilość paliwa.
        /// </summary>
        private int maxFuel = 100;
        /// <summary>
        /// Maksymalna wytrzymałość skafandra.
        /// </summary>
        private int maxDurability = 100;

        /// <summary>
        /// Ilość paliwa.
        /// </summary>
        public int Fuel
        {
            get
            {
                return fuel;
            }
            set
            {
                fuel = value;
            }
        }

        /// <summary>
        /// Wytrzymałość skafandra.
        /// Jeśli spadnie do zera, gra kończy się porażką gracza.
        /// </summary>
        public int Durability
        {
            get
            {
                return durability;
            }
            set
            {
                durability = value;
            }
        }

        /// <summary>
        /// Ilość paliwa.
        /// </summary>
        public int MaxFuel
        {
            get
            {
                return maxFuel;
            }
        }
        /// <summary>
        /// Maksymalna wytrzymałość skafandra.
        /// </summary>
        public int MaxDurability
        {
            get
            {
                return maxDurability;
            }
        }

        /// <summary>
        /// Metoda dodająca zdobytą ilość paliwa do zbiornika skafandra.
        /// </summary>
        /// <param name="fuel">Bonus typu "Paliwo" zdobyty przez gracza</param>
        public void AddFuel(Fuel fuel)
        {
            int amount = (int)(maxFuel * ((float)fuel.Amount / 100f));
            Console.WriteLine("Fuel amount: {0}, added: {1}", fuel.Amount, amount);
            if (this.fuel + amount <= maxFuel)
                this.fuel += amount;
            else
                this.fuel = maxFuel;

            Console.WriteLine("Fuel: {0}", this.fuel);
        }

        /// <summary>
        /// Metoda wywoływana po zdobyciu ilości doświadczenia wystarczającej do zwiększenia poziomu.
        /// Po jej wywołaniu maksymalna ilość paliwa oraz wytrzymałości jest przeskalowywana przez 1.5.
        /// </summary>
        public void IncreaseNanosuitLevel()
        {
            this.maxFuel = (int)(this.maxFuel * 1.5);
            this.maxDurability *= (int)(this.maxDurability * 1.5);
            this.fuel = (int)(this.fuel * 1.5);
            this.durability *= (int)(this.durability * 1.5);
        }
    }
}
