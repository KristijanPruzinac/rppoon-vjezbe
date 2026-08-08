using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.C1
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti C1 =======

    public class SupportTicket
    {
        public SupportTicket(string subject, int severity)
        {
            this.Subject = subject;
            this.Severity = severity;
        }

        public string Subject { get; private set; }

        public int Severity { get; private set; }

        public IList<string> Trail { get; } = new List<string>();
    }

    public abstract class SupportHandler
    {
        protected SupportHandler next;

        public void SetNext(SupportHandler handler)
        {
            this.next = handler;
        }

        public abstract string Handle(SupportTicket ticket);

        protected string Eskaliraj(SupportTicket ticket)
        {
            return this.next == null ? null : this.next.Handle(ticket);
        }
    }

    public class Level1Support : SupportHandler
    {
        public override string Handle(SupportTicket ticket)
        {
            ticket.Trail.Add("Level1Support");

            if (ticket.Severity <= 1)
            {
                return "Level1Support";
            }

            return this.Eskaliraj(ticket);
        }
    }

    public class Level2Support : SupportHandler
    {
        public override string Handle(SupportTicket ticket)
        {
            ticket.Trail.Add("Level2Support");

            if (ticket.Severity <= 3)
            {
                return "Level2Support";
            }

            return this.Eskaliraj(ticket);
        }
    }

    public class EngineeringTeam : SupportHandler
    {
        public override string Handle(SupportTicket ticket)
        {
            ticket.Trail.Add("EngineeringTeam");
            return "EngineeringTeam";
        }
    }
}
