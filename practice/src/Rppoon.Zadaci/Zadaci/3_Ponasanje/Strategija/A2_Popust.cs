using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Strategija.A2
{
    // ============================================================
    //  STRATEGIJA - razina A (vodeno), zadatak 2: Popusti u kosarici
    // ============================================================
    //  Kosarica je gotova. Napisi tri nacina obracuna popusta.
    //
    //  Obrati paznju: popust s postotkom i popust s fiksnim iznosom
    //  trebaju RAZLICITE podatke. Zato ih strategija prima kroz
    //  vlastiti konstruktor, a kosarica o tome ne zna nista.
    // ============================================================

    public interface IDiscountStrategy
    {
        decimal Apply(decimal amount);
    }

    /// <summary>Bez popusta. Iznos prolazi nepromijenjen.</summary>
    public class NoDiscount : IDiscountStrategy
    {
        public decimal Apply(decimal amount)
        {
            // TODO: vrati iznos kakav jest.
            throw new NotImplementedException("NoDiscount.Apply");
        }
    }

    /// <summary>Postotni popust, npr. 20 % znaci da se placa 80 %.</summary>
    public class PercentageDiscount : IDiscountStrategy
    {
        private readonly decimal percent;

        public PercentageDiscount(decimal percent)
        {
            this.percent = percent;
        }

        public decimal Apply(decimal amount)
        {
            // TODO: umanji iznos za 'percent' posto.
            throw new NotImplementedException("PercentageDiscount.Apply");
        }
    }

    /// <summary>Fiksni popust. Ukupno nikad ne smije pasti ispod nule.</summary>
    public class FixedDiscount : IDiscountStrategy
    {
        private readonly decimal value;

        public FixedDiscount(decimal value)
        {
            this.value = value;
        }

        public decimal Apply(decimal amount)
        {
            // TODO: oduzmi 'value', ali ne dopusti negativan iznos.
            throw new NotImplementedException("FixedDiscount.Apply");
        }
    }

    /// <summary>Kontekst. DAN JE U CIJELOSTI.</summary>
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

        /// <summary>Zbroj stavki, prije popusta.</summary>
        public decimal Subtotal()
        {
            decimal zbroj = 0m;
            foreach (decimal cijena in this.prices)
            {
                zbroj += cijena;
            }
            return zbroj;
        }

        /// <summary>Za naplatu. Kosarica ne zna KOJI je popust - samo ga primijeni.</summary>
        public decimal Total()
        {
            return this.Discount.Apply(this.Subtotal());
        }
    }
}
