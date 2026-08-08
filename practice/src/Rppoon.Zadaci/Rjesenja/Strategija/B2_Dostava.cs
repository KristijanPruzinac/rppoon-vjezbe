using System;

namespace Rppoon.Zadaci.Strategija.B2
{
    // ============ REFERENTNO RJESENJE - Strategija B2 ============

    public interface IShippingStrategy
    {
        decimal Cost(decimal weightKg);
    }

    public class StandardShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            return weightKg * 2.50m;
        }
    }

    public class ExpressShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            return (weightKg * 5.00m) + 3.00m;
        }
    }

    public class PickupShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            return 0m;
        }
    }

    public class Order
    {
        private readonly decimal price;
        private readonly decimal weightKg;

        public IShippingStrategy Shipping { get; set; }

        public Order(decimal price, decimal weightKg, IShippingStrategy shipping)
        {
            this.price = price;
            this.weightKg = weightKg;
            this.Shipping = shipping;
        }

        public decimal Total()
        {
            return this.price + this.Shipping.Cost(this.weightKg);
        }
    }
}
