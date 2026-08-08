using System;
using System.Linq;

namespace Rppoon.Zadaci.Strategija.C2
{
    // ===== ALTERNATIVNO RJESENJE - Strategija C2 =====
    //
    // Drugaciji put do istog cilja: LINQ umjesto petlje, svojstvo umjesto
    // polja samo za citanje, izrazna tijela metoda, Check preko null-uvjeta.

    public interface IValidationStrategy
    {
        bool IsValid(string input);
    }

    public class NotEmptyValidation : IValidationStrategy
    {
        public bool IsValid(string input) => input != null && input.Trim().Length > 0;
    }

    public class MinLengthValidation : IValidationStrategy
    {
        public int MinLength { get; private set; }

        public MinLengthValidation(int minLength)
        {
            this.MinLength = minLength;
        }

        public bool IsValid(string input) => (input?.Length ?? 0) >= this.MinLength;
    }

    public class DigitRequiredValidation : IValidationStrategy
    {
        public bool IsValid(string input) => input != null && input.Any(char.IsDigit);
    }

    public class RegistrationForm
    {
        private IValidationStrategy strategy;

        public RegistrationForm(IValidationStrategy strategy)
        {
            this.strategy = strategy;
        }

        public IValidationStrategy Strategy
        {
            get { return this.strategy; }
            set { this.strategy = value; }
        }

        public bool Check(string input) => this.strategy.IsValid(input);
    }
}
