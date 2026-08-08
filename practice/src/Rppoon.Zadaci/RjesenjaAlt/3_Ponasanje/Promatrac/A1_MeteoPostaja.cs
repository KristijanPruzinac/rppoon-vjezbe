using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Promatrac.A1
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac A1 =====
    //
    // Obavjescivanje ide preko kopije popisa (ToList), sto je zapravo
    // otpornije: promatrac se smije odjaviti dok traje obavjescivanje,
    // a da petlja ne pukne. Odjava ide preko provjere sadrzi li popis.

    public interface IWeatherObserver
    {
        void OnTemperatureChanged(double celsius);
    }

    public interface IWeatherStation
    {
        void Attach(IWeatherObserver observer);
        void Detach(IWeatherObserver observer);
        void Notify();
    }

    public class WeatherStation : IWeatherStation
    {
        private readonly HashSet<IWeatherObserver> observers = new HashSet<IWeatherObserver>();

        public double Temperature { get; private set; }

        public void SetTemperature(double celsius)
        {
            this.Temperature = celsius;
            this.Notify();
        }

        public void Attach(IWeatherObserver observer)
        {
            if (observer != null)
            {
                this.observers.Add(observer);
            }
        }

        public void Detach(IWeatherObserver observer)
        {
            if (this.observers.Contains(observer))
            {
                this.observers.Remove(observer);
            }
        }

        public void Notify()
        {
            foreach (IWeatherObserver observer in this.observers.ToList())
            {
                observer.OnTemperatureChanged(this.Temperature);
            }
        }
    }

    public class DisplayObserver : IWeatherObserver
    {
        public double Last { get; private set; } = double.NaN;

        public void OnTemperatureChanged(double celsius) => this.Last = celsius;
    }

    public class LoggerObserver : IWeatherObserver
    {
        private readonly List<double> entries = new List<double>();

        public IReadOnlyList<double> Entries => this.entries;

        public void OnTemperatureChanged(double celsius) => this.entries.Add(celsius);
    }
}
