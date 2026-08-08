using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.A1
{
    // ============================================================
    //  PROMATRAC - razina A (vodeno), zadatak 1: Meteo postaja
    // ============================================================
    //  Promatraci su vec napisani. Tvoj posao je subjekt: pretplata,
    //  odjava i obavjescivanje.
    //
    //  Kljucno: WeatherStation ne smije znati NISTA o tome tko je
    //  pretplacen. Vidi samo IWeatherObserver.
    // ============================================================

    /// <summary>Promatrac - onaj koga promjena zanima.</summary>
    public interface IWeatherObserver
    {
        void OnTemperatureChanged(double celsius);
    }

    /// <summary>Subjekt - onaj koga se promatra.</summary>
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

        /// <summary>DANO: promjena stanja uvijek povlaci obavijest.</summary>
        public void SetTemperature(double celsius)
        {
            this.Temperature = celsius;
            this.Notify();
        }

        public void Attach(IWeatherObserver observer)
        {
            // TODO: dodaj promatraca na popis.
            throw new NotImplementedException("WeatherStation.Attach");
        }

        public void Detach(IWeatherObserver observer)
        {
            // TODO: makni promatraca s popisa.
            throw new NotImplementedException("WeatherStation.Detach");
        }

        public void Notify()
        {
            // TODO: javi SVIM pretplacenim promatracima trenutnu temperaturu.
            throw new NotImplementedException("WeatherStation.Notify");
        }
    }

    /// <summary>DAN: pamti zadnju primljenu vrijednost.</summary>
    public class DisplayObserver : IWeatherObserver
    {
        public double Last { get; private set; } = double.NaN;

        public void OnTemperatureChanged(double celsius)
        {
            this.Last = celsius;
        }
    }

    /// <summary>DAN: biljezi svaku primljenu vrijednost.</summary>
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
