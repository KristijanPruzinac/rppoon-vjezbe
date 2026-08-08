using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.A2
{
    // ============ REFERENTNO RJESENJE - Promatrac A2 ============

    public interface ICropTracker
    {
        void Update(decimal price);
    }

    public class CropStockMarket
    {
        private readonly List<ICropTracker> trackers = new List<ICropTracker>();

        public decimal Price { get; private set; }

        public void Add(ICropTracker tracker)
        {
            this.trackers.Add(tracker);
        }

        public void Remove(ICropTracker tracker)
        {
            this.trackers.Remove(tracker);
        }

        public void OnUpdate(decimal price)
        {
            this.Price = price;
            this.Notify();
        }

        public void Notify()
        {
            foreach (ICropTracker tracker in this.trackers)
            {
                tracker.Update(this.Price);
            }
        }
    }

    public class CropBuyer : ICropTracker
    {
        private readonly decimal threshold;

        public CropBuyer(decimal threshold)
        {
            this.threshold = threshold;
        }

        public int Purchases { get; private set; }

        public void Update(decimal price)
        {
            if (price <= this.threshold)
            {
                this.Purchases = this.Purchases + 1;
            }
        }
    }

    public class CropView : ICropTracker
    {
        public string Display { get; private set; } = "-";

        public void Update(decimal price)
        {
            this.Display = price.ToString("F2") + " EUR";
        }
    }

    public class CropLogger : ICropTracker
    {
        private readonly List<decimal> history = new List<decimal>();

        public IReadOnlyList<decimal> History
        {
            get { return this.history; }
        }

        public void Update(decimal price)
        {
            this.history.Add(price);
        }
    }
}
