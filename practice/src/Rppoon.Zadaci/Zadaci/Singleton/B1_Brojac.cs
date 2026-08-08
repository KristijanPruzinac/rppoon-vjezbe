using System;

namespace Rppoon.Zadaci.Singleton.B1
{
    // ============================================================
    //  SINGLETON - razina B (sastavi), zadatak 1: Brojac identifikatora
    // ============================================================
    //  Dobivas samo potpise. Sve ostalo je tvoje - ukljucujuci polje
    //  za primjerak i privatni konstruktor, kojih ovdje jos nema.
    //
    //  Trazi se:
    //    Next()  vraca 1, pa 2, pa 3 ... (nikad dvaput isti broj)
    //    Current vraca zadnji izdani broj, bez izdavanja novog
    //
    //  Zasto Singleton: ako bi svatko mogao napraviti svoj brojac,
    //  dva dijela programa izdala bi isti identifikator.
    // ============================================================

    public class IdGenerator
    {
        public static IdGenerator Instance
        {
            get { throw new NotImplementedException("IdGenerator.Instance"); }
        }

        /// <summary>Izdaje sljedeci identifikator.</summary>
        public int Next()
        {
            throw new NotImplementedException("IdGenerator.Next");
        }

        /// <summary>Zadnji izdani identifikator; prije prvog Next() je 0.</summary>
        public int Current
        {
            get { throw new NotImplementedException("IdGenerator.Current"); }
        }
    }
}
