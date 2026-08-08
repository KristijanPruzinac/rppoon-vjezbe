using System;

namespace Rppoon.Zadaci.Singleton.B1
{
    // ============ REFERENTNO RJESENJE - Singleton B1 ============

    public class IdGenerator
    {
        private static IdGenerator instance;

        private int zadnji;

        private IdGenerator()
        {
        }

        public static IdGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IdGenerator();
                }
                return instance;
            }
        }

        public int Next()
        {
            this.zadnji = this.zadnji + 1;
            return this.zadnji;
        }

        public int Current
        {
            get { return this.zadnji; }
        }
    }
}
