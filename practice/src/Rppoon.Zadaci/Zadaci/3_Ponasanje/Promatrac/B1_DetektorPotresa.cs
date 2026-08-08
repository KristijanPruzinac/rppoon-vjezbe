using System;

namespace Rppoon.Zadaci.Promatrac.B1
{
    // ============================================================
    //  PROMATRAC - razina B (sastavi), zadatak 1: Detektor potresa
    // ============================================================
    //  Dobivas oba sucelja i potpise. Tijela su tvoja.
    //
    //  Trazi se:
    //    EarthquakeDetector.Measure(double)  zapamti vrijednost i, SAMO ako
    //                                        je veca od prethodne, obavijesti
    //    Logger                              biljezi svaku primljenu vrijednost
    //    Alarm(prag)                         broji koliko je puta primio
    //                                        vrijednost iznad praga
    //
    //  Paznja: obavijest ide samo kad vrijednost RASTE. Prva izmjena se
    //  usporeduje s pocetnom vrijednoscu 0.
    // ============================================================

    public interface IWatcher
    {
        void OnDetection(double value);
    }

    public interface IDetector
    {
        void StartWatching(IWatcher watcher);
        void StopWatching(IWatcher watcher);
        void Notify();
    }

    public class EarthquakeDetector : IDetector
    {
        /// <summary>Zadnja izmjerena vrijednost; na pocetku 0.</summary>
        public double SensorValue
        {
            get { throw new NotImplementedException("EarthquakeDetector.SensorValue"); }
        }

        /// <summary>Zabiljezi mjerenje i obavijesti samo ako je veca od prethodne.</summary>
        public void Measure(double value)
        {
            throw new NotImplementedException("EarthquakeDetector.Measure");
        }

        public void StartWatching(IWatcher watcher)
        {
            throw new NotImplementedException("EarthquakeDetector.StartWatching");
        }

        public void StopWatching(IWatcher watcher)
        {
            throw new NotImplementedException("EarthquakeDetector.StopWatching");
        }

        public void Notify()
        {
            throw new NotImplementedException("EarthquakeDetector.Notify");
        }
    }

    public class Logger : IWatcher
    {
        public int Count
        {
            get { throw new NotImplementedException("Logger.Count"); }
        }

        public double LastValue
        {
            get { throw new NotImplementedException("Logger.LastValue"); }
        }

        public void OnDetection(double value)
        {
            throw new NotImplementedException("Logger.OnDetection");
        }
    }

    public class Alarm : IWatcher
    {
        public Alarm(double threshold)
        {
            throw new NotImplementedException("Alarm konstruktor");
        }

        /// <summary>Koliko je puta primljena vrijednost bila strogo iznad praga.</summary>
        public int Triggered
        {
            get { throw new NotImplementedException("Alarm.Triggered"); }
        }

        public void OnDetection(double value)
        {
            throw new NotImplementedException("Alarm.OnDetection");
        }
    }
}
