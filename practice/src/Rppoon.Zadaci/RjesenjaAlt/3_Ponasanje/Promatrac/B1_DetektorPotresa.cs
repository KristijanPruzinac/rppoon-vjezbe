using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Promatrac.B1
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac B1 =====
    //
    // Popis pratitelja u polju s rucnim pomicanjem nije potreban - ovdje
    // se koristi HashSet i obavjescivanje preko LINQ-a, a stanje detektora
    // cuva se u izricitom polju umjesto u automatskom svojstvu.

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
        private readonly HashSet<IWatcher> watchers = new HashSet<IWatcher>();
        private double sensorValue;

        public double SensorValue
        {
            get { return this.sensorValue; }
        }

        public void Measure(double value)
        {
            double prethodna = this.sensorValue;
            this.sensorValue = value;

            if (value > prethodna)
            {
                this.Notify();
            }
        }

        public void StartWatching(IWatcher watcher) => this.watchers.Add(watcher);

        public void StopWatching(IWatcher watcher) => this.watchers.Remove(watcher);

        public void Notify()
        {
            this.watchers.ToList().ForEach(w => w.OnDetection(this.sensorValue));
        }
    }

    public class Logger : IWatcher
    {
        private readonly List<double> primljene = new List<double>();

        public int Count => this.primljene.Count;

        public double LastValue => this.primljene.Count == 0 ? 0d : this.primljene[this.primljene.Count - 1];

        public void OnDetection(double value) => this.primljene.Add(value);
    }

    public class Alarm : IWatcher
    {
        private readonly double threshold;
        private int triggered;

        public Alarm(double threshold)
        {
            this.threshold = threshold;
        }

        public int Triggered
        {
            get { return this.triggered; }
        }

        public void OnDetection(double value)
        {
            if (value > this.threshold)
            {
                this.triggered++;
            }
        }
    }
}
