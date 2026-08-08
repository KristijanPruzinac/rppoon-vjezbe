using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.C2
{
    // ============ REFERENTNO RJESENJE - Promatrac C2 ============

    public interface IEnergyObserver
    {
        void OnEnergyChanged(double production, double consumption);
    }

    public class EnergyGrid
    {
        private readonly List<IEnergyObserver> observers = new List<IEnergyObserver>();

        public double Production { get; private set; }

        public double Consumption { get; private set; }

        public void SetProduction(double value)
        {
            this.Production = value;
            this.Notify();
        }

        public void SetConsumption(double value)
        {
            this.Consumption = value;
            this.Notify();
        }

        public void Attach(IEnergyObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Detach(IEnergyObserver observer)
        {
            this.observers.Remove(observer);
        }

        private void Notify()
        {
            foreach (IEnergyObserver observer in this.observers)
            {
                observer.OnEnergyChanged(this.Production, this.Consumption);
            }
        }
    }

    public class EnergyLogger : IEnergyObserver
    {
        public int Entries { get; private set; }

        public double LastProduction { get; private set; }

        public double LastConsumption { get; private set; }

        public void OnEnergyChanged(double production, double consumption)
        {
            this.Entries = this.Entries + 1;
            this.LastProduction = production;
            this.LastConsumption = consumption;
        }
    }

    public class BackupUnit : IEnergyObserver
    {
        public bool Activated { get; private set; }

        public int Activations { get; private set; }

        public void OnEnergyChanged(double production, double consumption)
        {
            if (production <= 0)
            {
                this.Activated = false;
                return;
            }

            bool treba = consumption >= production * 0.9;

            if (treba)
            {
                this.Activations = this.Activations + 1;
            }

            this.Activated = treba;
        }
    }
}
