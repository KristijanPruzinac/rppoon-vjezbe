using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.B2
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti B2 =======

    public class Ticket
    {
        private readonly List<string> decisions = new List<string>();

        public Ticket(string code, bool vip)
        {
            this.Code = code;
            this.Vip = vip;
        }

        public string Code { get; private set; }

        public bool Vip { get; private set; }

        public IReadOnlyList<string> Decisions
        {
            get { return this.decisions; }
        }

        public string Status
        {
            get { return this.decisions.Count == 0 ? null : this.decisions[this.decisions.Count - 1]; }
        }

        public void Grant(string reason)
        {
            this.decisions.Add(reason);
        }

        public void Reject(string reason)
        {
            this.decisions.Add(reason);
        }
    }

    public interface ITicketProcessor
    {
        void SetNext(ITicketProcessor processor);

        void Process(Ticket ticket);
    }

    public class CounterfeitProcessor : ITicketProcessor
    {
        private readonly ISet<string> invalidCodes;
        private ITicketProcessor next;

        public CounterfeitProcessor(ISet<string> invalidCodes)
        {
            this.invalidCodes = invalidCodes;
        }

        public void SetNext(ITicketProcessor processor)
        {
            this.next = processor;
        }

        public void Process(Ticket ticket)
        {
            if (this.invalidCodes.Contains(ticket.Code))
            {
                ticket.Reject("krivotvorina");
                return;
            }

            if (this.next != null)
            {
                this.next.Process(ticket);
            }
        }
    }

    public class VipProcessor : ITicketProcessor
    {
        private ITicketProcessor next;

        public void SetNext(ITicketProcessor processor)
        {
            this.next = processor;
        }

        public void Process(Ticket ticket)
        {
            if (ticket.Vip)
            {
                ticket.Grant("vip ulaz");
                return;
            }

            if (this.next != null)
            {
                this.next.Process(ticket);
            }
        }
    }

    public class StandardProcessor : ITicketProcessor
    {
        private ITicketProcessor next;

        public void SetNext(ITicketProcessor processor)
        {
            this.next = processor;
        }

        public void Process(Ticket ticket)
        {
            ticket.Grant("standardni ulaz");
        }
    }

    public class TicketingSystem
    {
        private readonly ITicketProcessor prvi;

        public TicketingSystem(ISet<string> invalidCodes)
        {
            CounterfeitProcessor krivotvorine = new CounterfeitProcessor(invalidCodes);
            VipProcessor vip = new VipProcessor();
            StandardProcessor standardni = new StandardProcessor();

            krivotvorine.SetNext(vip);
            vip.SetNext(standardni);

            this.prvi = krivotvorine;
        }

        public void Admit(Ticket ticket)
        {
            this.prvi.Process(ticket);
        }
    }
}
