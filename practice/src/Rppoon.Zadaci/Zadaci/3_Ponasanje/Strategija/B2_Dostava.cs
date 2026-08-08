using System;

namespace Rppoon.Zadaci.Strategija.B2
{
    // ============================================================
    //  STRATEGIJA - razina B (sastavi), zadatak 2: Cijena dostave
    // ============================================================
    //  Dobivas sucelje i potpise. Sve ostalo pises sam.
    //
    //  Cjenik:
    //    StandardShipping   2.50 EUR po kilogramu
    //    ExpressShipping    5.00 EUR po kilogramu + 3.00 EUR fiksno
    //    PickupShipping     0.00 EUR (kupac preuzima osobno)
    //
    //  Order.Total() = cijena robe + cijena dostave za tezinu posiljke.
    // ============================================================

    public interface IShippingStrategy
    {
        decimal Cost(decimal weightKg);
    }

    public class StandardShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            throw new NotImplementedException("StandardShipping.Cost");
        }
    }

    public class ExpressShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            throw new NotImplementedException("ExpressShipping.Cost");
        }
    }

    public class PickupShipping : IShippingStrategy
    {
        public decimal Cost(decimal weightKg)
        {
            throw new NotImplementedException("PickupShipping.Cost");
        }
    }

    /// <summary>Kontekst. Zna cijenu i tezinu, ne zna cjenik dostave.</summary>
    public class Order
    {
        public IShippingStrategy Shipping { get; set; }

        public Order(decimal price, decimal weightKg, IShippingStrategy shipping)
        {
            throw new NotImplementedException("Order konstruktor");
        }

        public decimal Total()
        {
            throw new NotImplementedException("Order.Total");
        }
    }
}
