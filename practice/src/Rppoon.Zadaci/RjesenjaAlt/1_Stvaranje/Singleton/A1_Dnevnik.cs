using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A1
{
    // ===== ALTERNATIVNO RJESENJE - Singleton A1 =====
    //
    // Gladno (eager) stvaranje umjesto lijenog: primjerak nastaje pri
    // inicijalizaciji razreda, pa provjera 'if (instance == null)' otpada.
    // Za studenta je to jednako tocan Singleton - i test to mora priznati.

    public class Logger
    {
        private static readonly Logger instance = new Logger();

        private readonly List<string> entries = new List<string>();

        private Logger()
        {
        }

        public static Logger Instance
        {
            get { return instance; }
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
