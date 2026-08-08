using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.A1
{
    // ============ REFERENTNO RJESENJE - Promatrac A1 ============

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
        private readonly List<IWeatherObserver> observers = new List<IWeatherObserver>();

        public double Temperature { get; private set; }

        public void SetTemperature(double celsius)
        {
            this.Temperature = celsius;
            this.Notify();
        }

        public void Attach(IWeatherObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Detach(IWeatherObserver observer)
        {
            this.observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IWeatherObserver observer in this.observers)
            {
                observer.OnTemperatureChanged(this.Temperature);
            }
        }
    }

    public class DisplayObserver : IWeatherObserver
    {
        public double Last { get; private set; } = double.NaN;

        public void OnTemperatureChanged(double celsius)
        {
            this.Last = celsius;
        }
    }

    public class LoggerObserver : IWeatherObserver
    {
        private readonly List<double> entries = new List<double>();

        public IReadOnlyList<double> Entries
        {
            get { return this.entries; }
        }

        public void OnTemperatureChanged(double celsius)
        {
            this.entries.Add(celsius);
        }
    }
}
