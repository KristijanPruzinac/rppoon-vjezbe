using System;

namespace Rppoon.Zadaci.Singleton.C2
{
    // ===== ALTERNATIVNO RJESENJE - Singleton C2 =====
    //
    // Lazy<T> umjesto rucne provjere, stanje u automatskim svojstvima s
    // privatnim postavljacem, Open pisan s pozitivnim uvjetom umjesto
    // ranog izlaza.

    public class DatabaseConnection
    {
        private static readonly Lazy<DatabaseConnection> lijena =
            new Lazy<DatabaseConnection>(() => new DatabaseConnection());

        private DatabaseConnection()
        {
        }

        public static DatabaseConnection Instance
        {
            get { return lijena.Value; }
        }

        public bool IsOpen { get; private set; }

        public int OpenCount { get; private set; }

        public void Open()
        {
            if (!this.IsOpen)
            {
                this.IsOpen = true;
                this.OpenCount++;
            }
        }

        public void Close()
        {
            this.IsOpen = false;
        }
    }
}
