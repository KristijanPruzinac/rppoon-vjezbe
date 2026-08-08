using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A2
{
    // ===== ALTERNATIVNO RJESENJE - Singleton A2 =====
    //
    // Primjerak se dodjeljuje u statickom konstruktoru umjesto u
    // deklaraciji polja, citanje ide preko TryGetValue, a Instance je
    // izrazno tijelo. Sve troje je jednako ispravno.

    public class AppConfig
    {
        private static readonly AppConfig instance;

        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        static AppConfig()
        {
            instance = new AppConfig();
        }

        private AppConfig()
        {
        }

        public static AppConfig Instance => instance;

        public void Set(string key, string value)
        {
            this.settings[key] = value;
        }

        public string Get(string key)
        {
            string vrijednost;
            return this.settings.TryGetValue(key, out vrijednost) ? vrijednost : null;
        }
    }
}
