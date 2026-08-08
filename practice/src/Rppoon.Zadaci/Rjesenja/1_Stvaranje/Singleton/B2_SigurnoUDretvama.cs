using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.B2
{
    // ============ REFERENTNO RJESENJE - Singleton B2 ============
    //
    // Dvostruko provjereno zakljucavanje: prva provjera stedi zakljucavanje
    // kad primjerak vec postoji, druga (unutar zakljucanog dijela) osigurava
    // da ga stvori samo jedna dretva.

    public class SessionCache
    {
        private static SessionCache instance;
        private static readonly object brava = new object();

        private readonly Dictionary<string, string> stavke = new Dictionary<string, string>();

        private SessionCache()
        {
        }

        public static SessionCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (brava)
                    {
                        if (instance == null)
                        {
                            instance = new SessionCache();
                        }
                    }
                }
                return instance;
            }
        }

        public void Put(string user, string value)
        {
            lock (this.stavke)
            {
                this.stavke[user] = value;
            }
        }

        public string Get(string user)
        {
            lock (this.stavke)
            {
                string vrijednost;
                return this.stavke.TryGetValue(user, out vrijednost) ? vrijednost : null;
            }
        }
    }
}
