using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Promatrac.C2
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac C2 =====
    //
    // Stanje u izricitim poljima, obavjescivanje preko ForEach nad kopijom,
    // udio potrosnje izdvojen u imenovanu metodu umjesto uvjeta u tijelu.

    public interface IEnergyObserver
    {
        void OnEnergyChanged(double production, double consumption);
    }

    public class EnergyGrid
    {
        private readonly List<IEnergyObserver> promatraci = new List<IEnergyObserver>();
        private double proizvodnja;
        private double potrosnja;

        public double Production => this.proizvodnja;

        public double Consumption => this.potrosnja;

        public void SetProduction(double value)
        {
            this.proizvodnja = value;
            this.Obavijesti();
        }

        public void SetConsumption(double value)
        {
            this.potrosnja = value;
            this.Obavijesti();
        }

        public void Attach(IEnergyObserver observer)
        {
            if (!this.promatraci.Contains(observer))
            {
                this.promatraci.Add(observer);
            }
        }

        public void Detach(IEnergyObserver observer) => this.promatraci.Remove(observer);

        private void Obavijesti()
        {
            this.promatraci
                .ToList()
                .ForEach(p => p.OnEnergyChanged(this.proizvodnja, this.potrosnja));
        }
    }

    public class EnergyLogger : IEnergyObserver
    {
        private readonly List<(double Proizvodnja, double Potrosnja)> zapisi =
            new List<(double, double)>();

        public int Entries => this.zapisi.Count;

        public double LastProduction => this.zapisi.Count == 0 ? 0d : this.zapisi.Last().Proizvodnja;

        public double LastConsumption => this.zapisi.Count == 0 ? 0d : this.zapisi.Last().Potrosnja;

        public void OnEnergyChanged(double production, double consumption)
        {
            this.zapisi.Add((production, consumption));
        }
    }

    public class BackupUnit : IEnergyObserver
    {
        private bool aktivirana;
        private int brojAktivacija;

        public bool Activated => this.aktivirana;

        public int Activations => this.brojAktivacija;

        private static bool PrelaziGranicu(double proizvodnja, double potrosnja)
        {
            if (proizvodnja <= 0)
            {
                return false;
            }
            return potrosnja / proizvodnja >= 0.9;
        }

        public void OnEnergyChanged(double production, double consumption)
        {
            bool granica = PrelaziGranicu(production, consumption);

            if (granica)
            {
                this.brojAktivacija++;
            }

            this.aktivirana = granica;
        }
    }
}
