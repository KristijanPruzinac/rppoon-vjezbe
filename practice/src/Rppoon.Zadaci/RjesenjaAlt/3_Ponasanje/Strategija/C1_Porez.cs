using System;

namespace Rppoon.Zadaci.Strategija.C1
{
    // ===== ALTERNATIVNO RJESENJE - Strategija C1 =====
    //
    // Namjerno napisano drugacije od referentnog:
    //   - strategije dijele apstraktni bazni razred sa stopom,
    //     umjesto da svaka racuna svoj umnozak
    //   - Invoice drzi strategiju u izricitom polju, a ne u
    //     automatskom svojstvu
    //   - Total() racuna preko medurezultata
    //
    // Ako testovi prihvate i ovo i referentno rjesenje, znaci da
    // provjeravaju obrazac, a ne moj nacin pisanja.

    public interface ITaxStrategy
    {
        decimal Calculate(decimal amount);
    }

    /// <summary>Zajednicka osnovica: sve stope rade isto, razlikuje se broj.</summary>
    public abstract class TaxBase : ITaxStrategy
    {
        protected abstract decimal Rate { get; }

        public decimal Calculate(decimal amount)
        {
            return decimal.Round(amount * this.Rate, 2);
        }
    }

    public class StandardTax : TaxBase
    {
        protected override decimal Rate
        {
            get { return 0.25m; }
        }
    }

    public class ReducedTax : TaxBase
    {
        protected override decimal Rate
        {
            get { return 0.13m; }
        }
    }

    public class ZeroTax : TaxBase
    {
        protected override decimal Rate
        {
            get { return 0.00m; }
        }
    }

    public class Invoice
    {
        private readonly decimal amount;
        private ITaxStrategy strategy;

        public Invoice(decimal amount, ITaxStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }

            this.amount = amount;
            this.strategy = strategy;
        }

        public ITaxStrategy Strategy
        {
            get { return this.strategy; }
            set { this.strategy = value; }
        }

        public decimal Total()
        {
            decimal porez = this.strategy.Calculate(this.amount);
            return this.amount + porez;
        }
    }
}
