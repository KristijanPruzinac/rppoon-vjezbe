using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rppoon.Zadaci.Promatrac.A2
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac A2 =====
    //
    // Kupac broji preko ternarnog izraza, prikaz slaze niz interpolacijom,
    // dnevnik cuva povijest u redu (Queue) pa je izlaze kao popis.

    public interface ICropTracker
    {
        void Update(decimal price);
    }

    public class CropStockMarket
    {
        private readonly List<ICropTracker> trackers = new List<ICropTracker>();

        public decimal Price { get; private set; }

        public void Add(ICropTracker tracker) => this.trackers.Add(tracker);

        public void Remove(ICropTracker tracker) => this.trackers.Remove(tracker);

        public void OnUpdate(decimal price)
        {
            this.Price = price;
            this.Notify();
        }

        public void Notify()
        {
            for (int i = 0; i < this.trackers.Count; i++)
            {
                this.trackers[i].Update(this.Price);
            }
        }
    }

    public class CropBuyer : ICropTracker
    {
        private readonly decimal threshold;

        public CropBuyer(decimal threshold) => this.threshold = threshold;

        public int Purchases { get; private set; }

        public void Update(decimal price)
        {
            this.Purchases += price <= this.threshold ? 1 : 0;
        }
    }

    public class CropView : ICropTracker
    {
        public string Display { get; private set; } = "-";

        public void Update(decimal price)
        {
            this.Display = $"{price.ToString("F2", CultureInfo.CurrentCulture)} EUR";
        }
    }

    public class CropLogger : ICropTracker
    {
        private readonly Queue<decimal> history = new Queue<decimal>();

        public IReadOnlyList<decimal> History
        {
            get { return new List<decimal>(this.history); }
        }

        public void Update(decimal price) => this.history.Enqueue(price);
    }
}
