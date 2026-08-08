namespace Rppoon.Zadaci.Strategija.C2
{
    // ============ REFERENTNO RJESENJE - Strategija C2 ============

    public interface IValidationStrategy
    {
        bool IsValid(string input);
    }

    public class NotEmptyValidation : IValidationStrategy
    {
        public bool IsValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }

    public class MinLengthValidation : IValidationStrategy
    {
        private readonly int minLength;

        public MinLengthValidation(int minLength)
        {
            this.minLength = minLength;
        }

        public bool IsValid(string input)
        {
            return input != null && input.Length >= this.minLength;
        }
    }

    public class DigitRequiredValidation : IValidationStrategy
    {
        public bool IsValid(string input)
        {
            if (input == null)
            {
                return false;
            }

            foreach (char znak in input)
            {
                if (char.IsDigit(znak))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class RegistrationForm
    {
        public IValidationStrategy Strategy { get; set; }

        public RegistrationForm(IValidationStrategy strategy)
        {
            this.Strategy = strategy;
        }

        public bool Check(string input)
        {
            return this.Strategy.IsValid(input);
        }
    }
}
