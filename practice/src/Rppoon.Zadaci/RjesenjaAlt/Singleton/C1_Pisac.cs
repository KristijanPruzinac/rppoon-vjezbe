using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Singleton.C1
{
    // ===== ALTERNATIVNO RJESENJE - Singleton C1 =====
    //
    // Gladno stvaranje i obicna lista umjesto Queue<T>. Isto vanjsko
    // ponasanje, drukcija unutrasnjost - upravo ono sto test mora dopustiti.

    public class PrintSpooler
    {
        private static readonly PrintSpooler jedini = new PrintSpooler();

        private readonly List<string> dokumenti = new List<string>();

        private PrintSpooler()
        {
        }

        public static PrintSpooler Instance
        {
            get { return jedini; }
        }

        public void Send(string document)
        {
            this.dokumenti.Add(document);
        }

        public int Pending
        {
            get { return this.dokumenti.Count; }
        }

        public string PrintNext()
        {
            if (!this.dokumenti.Any())
            {
                return null;
            }

            string najstariji = this.dokumenti[0];
            this.dokumenti.RemoveAt(0);
            return najstariji;
        }
    }
}
