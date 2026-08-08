using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.C2
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti C2 =======

    public class Registration
    {
        public Registration(string username, string password, int age)
        {
            this.Username = username;
            this.Password = password;
            this.Age = age;
        }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public int Age { get; private set; }
    }

    public abstract class ValidationHandler
    {
        protected ValidationHandler next;

        public void SetNext(ValidationHandler handler)
        {
            this.next = handler;
        }

        public abstract IList<string> Validate(Registration registration);

        /// <summary>Pogreske ostatka lanca; prazno ako smo zadnji.</summary>
        protected IList<string> OstatakLanca(Registration registration)
        {
            return this.next == null ? new List<string>() : this.next.Validate(registration);
        }
    }

    public class UsernameValidator : ValidationHandler
    {
        public override IList<string> Validate(Registration registration)
        {
            List<string> pogreske = new List<string>();

            if (string.IsNullOrEmpty(registration.Username) || registration.Username.Length < 3)
            {
                pogreske.Add("korisnicko ime");
            }

            foreach (string ostala in this.OstatakLanca(registration))
            {
                pogreske.Add(ostala);
            }

            return pogreske;
        }
    }

    public class PasswordValidator : ValidationHandler
    {
        public override IList<string> Validate(Registration registration)
        {
            List<string> pogreske = new List<string>();

            if (registration.Password == null || registration.Password.Length < 8)
            {
                pogreske.Add("lozinka");
            }

            foreach (string ostala in this.OstatakLanca(registration))
            {
                pogreske.Add(ostala);
            }

            return pogreske;
        }
    }

    public class AgeValidator : ValidationHandler
    {
        public override IList<string> Validate(Registration registration)
        {
            List<string> pogreske = new List<string>();

            if (registration.Age < 18)
            {
                pogreske.Add("dob");
            }

            foreach (string ostala in this.OstatakLanca(registration))
            {
                pogreske.Add(ostala);
            }

            return pogreske;
        }
    }
}
