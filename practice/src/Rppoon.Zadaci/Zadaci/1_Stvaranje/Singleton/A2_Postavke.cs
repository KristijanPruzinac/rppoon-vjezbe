using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A2
{
    // ============================================================
    //  SINGLETON - razina A (vodeno), zadatak 2: Postavke aplikacije
    // ============================================================
    //  Druga inacica stvaranja: GLADNA (eager). Primjerak nastaje cim
    //  se razred prvi put dotakne, pa provjera "je li null" otpada.
    //
    //  Zauzvrat: nema kontrole KADA nastaje, i nastaje cak i ako ga
    //  nikad ne upotrijebis. Lijena inacica iz zadatka A1 radi obrnuto.
    //
    //  Dovrsi Instance, Set i Get.
    // ============================================================

    public class AppConfig
    {
        /// <summary>Gladno stvaranje - vrijednost se dodjeljuje odmah.</summary>
        private static readonly AppConfig instance = new AppConfig();

        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        private AppConfig()
        {
        }

        public static AppConfig Instance
        {
            get
            {
                // TODO: vrati jedini primjerak.
                throw new NotImplementedException("AppConfig.Instance");
            }
        }

        /// <summary>Zapisuje postavku. Postojeci kljuc se prepisuje.</summary>
        public void Set(string key, string value)
        {
            // TODO: spremi vrijednost pod zadanim kljucem.
            throw new NotImplementedException("AppConfig.Set");
        }

        /// <summary>Cita postavku; za nepoznat kljuc vraca null.</summary>
        public string Get(string key)
        {
            // TODO: vrati vrijednost ili null ako kljuc ne postoji.
            throw new NotImplementedException("AppConfig.Get");
        }
    }
}
