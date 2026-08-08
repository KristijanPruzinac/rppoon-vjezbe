using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.C1
{
    // ============ REFERENTNO RJESENJE - Singleton C1 ============

    public class PrintSpooler
    {
        private static PrintSpooler instance;

        private readonly Queue<string> red = new Queue<string>();

        private PrintSpooler()
        {
        }

        public static PrintSpooler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrintSpooler();
                }
                return instance;
            }
        }

        public void Send(string document)
        {
            this.red.Enqueue(document);
        }

        public int Pending
        {
            get { return this.red.Count; }
        }

        public string PrintNext()
        {
            if (this.red.Count == 0)
            {
                return null;
            }
            return this.red.Dequeue();
        }
    }
}
