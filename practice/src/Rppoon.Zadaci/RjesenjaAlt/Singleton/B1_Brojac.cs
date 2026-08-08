using System;

namespace Rppoon.Zadaci.Singleton.B1
{
    // ===== ALTERNATIVNO RJESENJE - Singleton B1 =====
    //
    // Gladno stvaranje, brojac kao svojstvo s privatnim postavljacem,
    // Next preko predinkrementa. Ista vanjska pravila, drugi kod.

    public class IdGenerator
    {
        private static readonly IdGenerator jedini = new IdGenerator();

        private IdGenerator()
        {
            this.Current = 0;
        }

        public static IdGenerator Instance
        {
            get { return jedini; }
        }

        public int Current { get; private set; }

        public int Next()
        {
            return this.Current = this.Current + 1;
        }
    }
}
