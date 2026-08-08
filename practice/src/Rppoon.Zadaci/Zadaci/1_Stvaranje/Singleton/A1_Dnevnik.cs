using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A1
{
    // ============================================================
    //  SINGLETON - razina A (vodeno), zadatak 1: Dnevnik (Logger)
    // ============================================================
    //  Tri stvari cine Singleton, i sve tri su ovdje vec pripremljene
    //  osim jedne: staticno polje, privatni konstruktor i staticna
    //  tocka pristupa.
    //
    //  Tvoj posao: dovrsi svojstvo Instance tako da se primjerak
    //  stvori SAMO prvi put (lijeno stvaranje).
    // ============================================================

    public class Logger
    {
        /// <summary>Jedini primjerak. Staticno - pripada razredu, ne objektu.</summary>
        private static Logger instance;

        private readonly List<string> entries = new List<string>();

        /// <summary>
        /// Privatni konstruktor. Ovo je ono sto sprjecava 'new Logger()'
        /// izvana - bez njega Singleton ne postoji, ma sto pisalo drugdje.
        /// </summary>
        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                // TODO: ako primjerak jos ne postoji, stvori ga; zatim ga vrati.
                throw new NotImplementedException("Logger.Instance");
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
