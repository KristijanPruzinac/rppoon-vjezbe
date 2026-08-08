using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.C1
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti C1 =====
    //
    // Odluka je razdvojena na dva koraka: "je li ovo moje?" i "obradi".
    // Prosljedivanje je napisano jednom, u bazi. EngineeringTeam ne
    // treba nista posebno - samo kaze da je sve njegovo.

    public class SupportTicket
    {
        private readonly List<string> trail = new List<string>();

        public SupportTicket(string subject, int severity)
        {
            this.Subject = subject;
            this.Severity = severity;
        }

        public string Subject { get; private set; }

        public int Severity { get; private set; }

        public IList<string> Trail => this.trail;
    }

    public abstract class SupportHandler
    {
        protected SupportHandler next;

        public void SetNext(SupportHandler handler)
        {
            this.next = handler;
        }

        protected abstract bool Nadlezan(SupportTicket ticket);

        public virtual string Handle(SupportTicket ticket)
        {
            ticket.Trail.Add(this.GetType().Name);

            if (this.Nadlezan(ticket))
            {
                return this.GetType().Name;
            }

            if (this.next != null)
            {
                return this.next.Handle(ticket);
            }

            return null;
        }
    }

    public class Level1Support : SupportHandler
    {
        protected override bool Nadlezan(SupportTicket ticket) => ticket.Severity <= 1;
    }

    public class Level2Support : SupportHandler
    {
        protected override bool Nadlezan(SupportTicket ticket) => ticket.Severity <= 3;
    }

    public class EngineeringTeam : SupportHandler
    {
        protected override bool Nadlezan(SupportTicket ticket) => true;
    }
}
