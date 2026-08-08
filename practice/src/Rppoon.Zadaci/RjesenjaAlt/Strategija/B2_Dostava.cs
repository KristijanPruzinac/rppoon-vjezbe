using System;

namespace Rppoon.Zadaci.Strategija.B2
{
    // ===== ALTERNATIVNO RJESENJE - Strategija B2 =====
    //
    // Cjenik u imenovanim konstantama, Order cuva podatke u svojstvima
    // i racuna zbroj preko medurezultata.

    public interface IShippingStrategy
    {
        decimal Cost(decimal weightKg);
    }

    public class StandardShipping : IShippingStrategy
    {
        private const decimal CijenaPoKilogramu = 2.50m;

        public decimal Cost(decimal weightKg)
        {
            return CijenaPoKilogramu * weightKg;
        }
    }

    public class ExpressShipping : IShippingStrategy
    {
        private const decimal CijenaPoKilogramu = 5.00m;
        private const decimal FiksniDodatak = 3.00m;

        public decimal Cost(decimal weightKg)
        {
            decimal poTezini = CijenaPoKilogramu * weightKg;
            return poTezini + FiksniDodatak;
        }
    }

    public class PickupShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            return decimal.Zero;
        }
    }

    public class Order
    {
        public decimal Price { get; private set; }

        public decimal WeightKg { get; private set; }

        public IShippingStrategy Shipping { get; set; }

        public Order(decimal price, decimal weightKg, IShippingStrategy shipping)
        {
            this.Price = price;
            this.WeightKg = weightKg;
            this.Shipping = shipping;
        }

        public decimal Total()
        {
            decimal dostava = this.Shipping.Cost(this.WeightKg);
            return this.Price + dostava;
        }
    }
}
