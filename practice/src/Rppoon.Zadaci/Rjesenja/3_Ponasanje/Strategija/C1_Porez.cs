namespace Rppoon.Zadaci.Strategija.C1
{
    // ============ REFERENTNO RJESENJE - Strategija C1 ============

    public interface ITaxStrategy
    {
        decimal Calculate(decimal amount);
    }

    public class StandardTax : ITaxStrategy
    {
        public decimal Calculate(decimal amount)
        {
            return amount * 0.25m;
        }
    }

    public class ReducedTax : ITaxStrategy
    {
        public decimal Calculate(decimal amount)
        {
            return amount * 0.13m;
        }
    }

    public class ZeroTax : ITaxStrategy
    {
        public decimal Calculate(decimal amount)
        {
            return 0m;
        }
    }

    public class Invoice
    {
        private readonly decimal amount;

        public ITaxStrategy Strategy { get; set; }

        public Invoice(decimal amount, ITaxStrategy strategy)
        {
            this.amount = amount;
            this.Strategy = strategy;
        }

        public decimal Total()
        {
            return this.amount + this.Strategy.Calculate(this.amount);
        }
    }
}
