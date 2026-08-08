using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Singleton.A2
{
    // ============ REFERENTNO RJESENJE - Singleton A2 ============

    public class AppConfig
    {
        private static readonly AppConfig instance = new AppConfig();

        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        private AppConfig()
        {
        }

        public static AppConfig Instance
        {
            get { return instance; }
        }

        public void Set(string key, string value)
        {
            this.settings[key] = value;
        }

        public string Get(string key)
        {
            if (this.settings.ContainsKey(key))
            {
                return this.settings[key];
            }
            return null;
        }
    }
}
