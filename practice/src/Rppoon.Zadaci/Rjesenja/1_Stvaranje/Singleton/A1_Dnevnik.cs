using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A1
{
    // ============ REFERENTNO RJESENJE - Singleton A1 ============

    public class Logger
    {
        private static Logger instance;

        private readonly List<string> entries = new List<string>();

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        public void Log(string message)
        {
            this.entries.Add(message);
        }

        public IReadOnlyList<string> Entries
        {
            get { return this.entries; }
        }
    }
}
