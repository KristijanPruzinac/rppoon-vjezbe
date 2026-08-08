namespace Rppoon.Zadaci.Singleton.C2
{
    // ============ REFERENTNO RJESENJE - Singleton C2 ============

    public class DatabaseConnection
    {
        private static DatabaseConnection instance;

        private bool otvorena;
        private int brojOtvaranja;

        private DatabaseConnection()
        {
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseConnection();
                }
                return instance;
            }
        }

        public void Open()
        {
            if (this.otvorena)
            {
                return;
            }

            this.otvorena = true;
            this.brojOtvaranja = this.brojOtvaranja + 1;
        }

        public void Close()
        {
            this.otvorena = false;
        }

        public bool IsOpen
        {
            get { return this.otvorena; }
        }

        public int OpenCount
        {
            get { return this.brojOtvaranja; }
        }
    }
}
