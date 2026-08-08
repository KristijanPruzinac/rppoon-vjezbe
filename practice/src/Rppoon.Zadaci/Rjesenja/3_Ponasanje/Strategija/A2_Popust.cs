using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Strategija.A2
{
    // ============ REFERENTNO RJESENJE - Strategija A2 ============

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
        private readonly decimal percent;

        public PercentageDiscount(decimal percent)
        {
            this.percent = percent;
        }

        public decimal Apply(decimal amount)
        {
            return amount - (amount * this.percent / 100m);
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
            decimal umanjeno = amount - this.value;
            return umanjeno < 0m ? 0m : umanjeno;
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
            decimal zbroj = 0m;
            foreach (decimal cijena in this.prices)
            {
                zbroj += cijena;
            }
            return zbroj;
        }

        public decimal Total()
        {
            return this.Discount.Apply(this.Subtotal());
        }
    }
}
