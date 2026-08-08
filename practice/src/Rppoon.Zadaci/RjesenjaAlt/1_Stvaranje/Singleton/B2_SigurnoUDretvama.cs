using System;
using System.Collections.Concurrent;

namespace Rppoon.Zadaci.Singleton.B2
{
    // ===== ALTERNATIVNO RJESENJE - Singleton B2 =====
    //
    // Bez ijednog 'lock': Lazy<T> sam jamci da ce tvornicu pozvati tocno
    // jedna dretva, a ConcurrentDictionary rjesava istovremeni upis.
    // Kratko i ispravno - test to mora priznati jednako kao zakljucavanje.

    public class SessionCache
    {
        private static readonly Lazy<SessionCache> lijeni =
            new Lazy<SessionCache>(() => new SessionCache());

        private readonly ConcurrentDictionary<string, string> stavke =
            new ConcurrentDictionary<string, string>();

        private SessionCache()
        {
        }

        public static SessionCache Instance
        {
            get { return lijeni.Value; }
        }

        public void Put(string user, string value)
        {
            this.stavke[user] = value;
        }

        public string Get(string user)
        {
            string vrijednost;
            return this.stavke.TryGetValue(user, out vrijednost) ? vrijednost : null;
        }
    }
}
