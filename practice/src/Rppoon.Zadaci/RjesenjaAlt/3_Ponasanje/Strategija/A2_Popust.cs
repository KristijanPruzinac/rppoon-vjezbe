using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Strategija.A2
{
    // ===== ALTERNATIVNO RJESENJE - Strategija A2 =====
    //
    // Postotak preko mnozitelja umjesto oduzimanja, fiksni popust preko
    // Math.Max, zbroj stavki preko LINQ-a.

    public interface IDiscountStrategy
    {
        decimal Apply(decimal amount);
    }

    public class NoDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal amount)
        {
            return amount;
        }
    }

    public class PercentageDiscount : IDiscountStrategy
    {
        private readonly decimal mnozitelj;

        public PercentageDiscount(decimal percent)
        {
            this.mnozitelj = 1m - (percent / 100m);
        }

        public decimal Apply(decimal amount)
        {
            return amount * this.mnozitelj;
        }
    }

    public class FixedDiscount : IDiscountStrategy
    {
        private readonly decimal value;

        public FixedDiscount(decimal value)
        {
            this.value = value;
        }

        public decimal Apply(decimal amount)
        {
            return Math.Max(0m, amount - this.value);
        }
    }

    public class Cart
    {
        private readonly List<decimal> prices = new List<decimal>();

        public IDiscountStrategy Discount { get; set; }

        public Cart(IDiscountStrategy discount)
        {
            this.Discount = discount;
        }

        public void AddItem(decimal price)
        {
            this.prices.Add(price);
        }

        public decimal Subtotal()
        {
            return this.prices.Sum();
        }

        public decimal Total()
        {
            return this.Discount.Apply(this.Subtotal());
        }
    }
}
