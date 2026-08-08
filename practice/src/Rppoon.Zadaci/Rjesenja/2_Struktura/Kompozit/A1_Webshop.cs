using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.A1
{
    // ============ REFERENTNO RJESENJE - Kompozit A1 ============

    public interface IBuyable
    {
        string Name { get; }
        decimal CalculatePriceWithTax();
    }

    public class Product : IBuyable
    {
        private readonly decimal price;

        public Product(string name, decimal price)
        {
            this.Name = name;
            this.price = price;
        }

        public string Name { get; private set; }

        public decimal CalculatePriceWithTax()
        {
            return this.price * 1.25m;
        }
    }

    public class GiftSet : IBuyable
    {
        private readonly List<IBuyable> items = new List<IBuyable>();

        public GiftSet(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public int Count
        {
            get { return this.items.Count; }
        }

        public void Add(IBuyable item)
        {
            this.items.Add(item);
        }

        public void Remove(IBuyable item)
        {
            this.items.Remove(item);
        }

        public decimal CalculatePriceWithTax()
        {
            decimal ukupno = 0m;
            foreach (IBuyable item in this.items)
            {
                ukupno += item.CalculatePriceWithTax();
            }
            return ukupno;
        }
    }
}
