using System;

namespace Rppoon.Zadaci.Singleton.B2
{
    // ============================================================
    //  SINGLETON - razina B (sastavi), zadatak 2: Siguran u vise dretvi
    // ============================================================
    //  Lijeni Singleton iz zadatka A1 ima rupu. Dvije dretve mogu
    //  istovremeno proci provjeru "je li null" prije nego ijedna
    //  stigne dodijeliti vrijednost - i nastanu DVA primjerka.
    //
    //  Nije teorija: u ovom projektu je izmjereno 8 razlicitih
    //  primjeraka na 50 istovremenih poziva.
    //
    //  Napisi SessionCache tako da i pod 200 istovremenih poziva
    //  postoji tocno jedan primjerak.
    //
    //  Dopusteno je bilo koje ispravno rjesenje: zakljucavanje (lock),
    //  dvostruko provjereno zakljucavanje, gladno stvaranje ili Lazy<T>.
    // ============================================================

    public class SessionCache
    {
        public static SessionCache Instance
        {
            get { throw new NotImplementedException("SessionCache.Instance"); }
        }

        /// <summary>Sprema vrijednost za korisnika.</summary>
        public void Put(string user, string value)
        {
            throw new NotImplementedException("SessionCache.Put");
        }

        /// <summary>Vraca spremljenu vrijednost ili null.</summary>
        public string Get(string user)
        {
            throw new NotImplementedException("SessionCache.Get");
        }
    }
}
