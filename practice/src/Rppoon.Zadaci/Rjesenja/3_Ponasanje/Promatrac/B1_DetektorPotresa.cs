using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.B1
{
    // ============ REFERENTNO RJESENJE - Promatrac B1 ============

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
        private readonly List<IWatcher> watchers = new List<IWatcher>();

        public double SensorValue { get; private set; }

        public void Measure(double value)
        {
            bool shouldNotify = value > this.SensorValue;
            this.SensorValue = value;

            if (shouldNotify)
            {
                this.Notify();
            }
        }

        public void StartWatching(IWatcher watcher)
        {
            this.watchers.Add(watcher);
        }

        public void StopWatching(IWatcher watcher)
        {
            this.watchers.Remove(watcher);
        }

        public void Notify()
        {
            foreach (IWatcher watcher in this.watchers)
            {
                watcher.OnDetection(this.SensorValue);
            }
        }
    }

    public class Logger : IWatcher
    {
        public int Count { get; private set; }

        public double LastValue { get; private set; }

        public void OnDetection(double value)
        {
            this.Count = this.Count + 1;
            this.LastValue = value;
        }
    }

    public class Alarm : IWatcher
    {
        private readonly double threshold;

        public Alarm(double threshold)
        {
            this.threshold = threshold;
        }

        public int Triggered { get; private set; }

        public void OnDetection(double value)
        {
            if (value > this.threshold)
            {
                this.Triggered = this.Triggered + 1;
            }
        }
    }
}
