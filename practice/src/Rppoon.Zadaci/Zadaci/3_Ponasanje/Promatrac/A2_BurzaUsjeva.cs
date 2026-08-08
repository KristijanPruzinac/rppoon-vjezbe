using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.A2
{
    // ============================================================
    //  PROMATRAC - razina A (vodeno), zadatak 2: Burza usjeva
    // ============================================================
    //  Obrnuto od zadatka A1: subjekt je gotov, promatrace pises ti.
    //
    //  Isti dogadaj, tri razlicite reakcije - i nijedan promatrac ne
    //  zna da postoje ostali. To je cijela vrijednost obrasca.
    // ============================================================

    public interface ICropTracker
    {
        void Update(decimal price);
    }

    /// <summary>Subjekt. DAN JE U CIJELOSTI.</summary>
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

    /// <summary>Kupuje cim cijena padne na prag ili ispod njega.</summary>
    public class CropBuyer : ICropTracker
    {
        private readonly decimal threshold;

        public CropBuyer(decimal threshold)
        {
            this.threshold = threshold;
        }

        /// <summary>Koliko je puta kupac kupio.</summary>
        public int Purchases { get; private set; }

        public void Update(decimal price)
        {
            // TODO: ako je cijena <= praga, povecaj broj kupnji.
            throw new NotImplementedException("CropBuyer.Update");
        }
    }

    /// <summary>Pamti zadnju cijenu za prikaz, zaokruzenu na dvije decimale.</summary>
    public class CropView : ICropTracker
    {
        public string Display { get; private set; } = "-";

        public void Update(decimal price)
        {
            // TODO: postavi Display na cijenu s dvije decimale i oznakom " EUR",
            //       npr. 3.5m -> "3,50 EUR" ovisno o kulturi. Koristi
            //       price.ToString("F2") + " EUR" da rezultat bude predvidljiv.
            throw new NotImplementedException("CropView.Update");
        }
    }

    /// <summary>Biljezi svaku promjenu cijene.</summary>
    public class CropLogger : ICropTracker
    {
        private readonly List<decimal> history = new List<decimal>();

        public IReadOnlyList<decimal> History
        {
            get { return this.history; }
        }

        public void Update(decimal price)
        {
            // TODO: zabiljezi cijenu.
            throw new NotImplementedException("CropLogger.Update");
        }
    }
}
