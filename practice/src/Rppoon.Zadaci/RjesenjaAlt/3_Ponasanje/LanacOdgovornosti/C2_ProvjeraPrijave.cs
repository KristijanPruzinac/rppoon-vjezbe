using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.LanacOdgovornosti.C2
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti C2 =====
    //
    // Baza pita pravilo samo za JEDNU pogresku (ili null), pa sama
    // spaja rezultat s ostatkom lanca. Konkretna pravila vise ne
    // moraju znati da lanac uopce postoji.

    public class Registration
    {
        private readonly string username;
        private readonly string password;
        private readonly int age;

        public Registration(string username, string password, int age)
        {
            this.username = username;
            this.password = password;
            this.age = age;
        }

        public string Username => this.username;

        public string Password => this.password;

        public int Age => this.age;
    }

    public abstract class ValidationHandler
    {
        protected ValidationHandler next;

        public void SetNext(ValidationHandler handler)
        {
            this.next = handler;
        }

        /// <summary>Naziv pogreske, ili null ako je ovo pravilo zadovoljeno.</summary>
        protected abstract string Pogreska(Registration registration);

        public virtual IList<string> Validate(Registration registration)
        {
            IEnumerable<string> moja = new[] { this.Pogreska(registration) }.Where(p => p != null);
            IEnumerable<string> ostale = this.next?.Validate(registration) ?? Enumerable.Empty<string>();

            return moja.Concat(ostale).ToList();
        }
    }

    public class UsernameValidator : ValidationHandler
    {
        protected override string Pogreska(Registration registration)
        {
            bool prekratko = string.IsNullOrWhiteSpace(registration.Username)
                             || registration.Username.Length < 3;
            return prekratko ? "korisnicko ime" : null;
        }
    }

    public class PasswordValidator : ValidationHandler
    {
        protected override string Pogreska(Registration registration)
        {
            int duljina = registration.Password?.Length ?? 0;
            return duljina < 8 ? "lozinka" : null;
        }
    }

    public class AgeValidator : ValidationHandler
    {
        protected override string Pogreska(Registration registration)
        {
            return registration.Age >= 18 ? null : "dob";
        }
    }
}
