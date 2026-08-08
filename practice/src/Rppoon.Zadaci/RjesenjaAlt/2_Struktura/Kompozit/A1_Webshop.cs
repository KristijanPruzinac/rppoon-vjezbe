using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.A1
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit A1 =====
    //
    // Zbrajanje preko LINQ-a umjesto petlje, ime u izricitom polju,
    // porez kao imenovana konstanta.

    public interface IBuyable
    {
        string Name { get; }
        decimal CalculatePriceWithTax();
    }

    public class Product : IBuyable
    {
        private const decimal StopaPoreza = 0.25m;

        private readonly string name;
        private readonly decimal price;

        public Product(string name, decimal price)
        {
            this.name = name;
            this.price = price;
        }

        public string Name => this.name;

        public decimal CalculatePriceWithTax() => this.price * (1m + StopaPoreza);
    }

    public class GiftSet : IBuyable
    {
        private readonly string name;
        private readonly List<IBuyable> sadrzaj = new List<IBuyable>();

        public GiftSet(string name)
        {
            this.name = name;
        }

        public string Name => this.name;

        public int Count => this.sadrzaj.Count;

        public void Add(IBuyable item)
        {
            this.sadrzaj.Add(item);
        }

        public void Remove(IBuyable item)
        {
            this.sadrzaj.Remove(item);
        }

        public decimal CalculatePriceWithTax()
        {
            return this.sadrzaj.Sum(stavka => stavka.CalculatePriceWithTax());
        }
    }
}
