using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.LanacOdgovornosti.B2
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti B2 =====
    //
    // Karike se vezu ?. operatorom, a klijent lanac slaze iz polja
    // obradivaca u petlji umjesto rucnim nizanjem. Rezultat je isti
    // lanac; klijent i dalje pamti samo prvu kariku.

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

        public IReadOnlyList<string> Decisions => this.decisions.AsReadOnly();

        public string Status => this.decisions.LastOrDefault();

        public void Grant(string reason) => this.decisions.Add(reason);

        public void Reject(string reason) => this.decisions.Add(reason);
    }

    public interface ITicketProcessor
    {
        void SetNext(ITicketProcessor processor);

        void Process(Ticket ticket);
    }

    public class CounterfeitProcessor : ITicketProcessor
    {
        private readonly ISet<string> invalidCodes;

        public CounterfeitProcessor(ISet<string> invalidCodes)
        {
            this.invalidCodes = invalidCodes;
        }

        public ITicketProcessor Sljedeci { get; private set; }

        public void SetNext(ITicketProcessor processor) => this.Sljedeci = processor;

        public void Process(Ticket ticket)
        {
            if (this.invalidCodes.Contains(ticket.Code))
            {
                ticket.Reject("krivotvorina");
            }
            else
            {
                this.Sljedeci?.Process(ticket);
            }
        }
    }

    public class VipProcessor : ITicketProcessor
    {
        public ITicketProcessor Sljedeci { get; private set; }

        public void SetNext(ITicketProcessor processor) => this.Sljedeci = processor;

        public void Process(Ticket ticket)
        {
            if (ticket.Vip)
            {
                ticket.Grant("vip ulaz");
            }
            else
            {
                this.Sljedeci?.Process(ticket);
            }
        }
    }

    public class StandardProcessor : ITicketProcessor
    {
        public ITicketProcessor Sljedeci { get; private set; }

        public void SetNext(ITicketProcessor processor) => this.Sljedeci = processor;

        public void Process(Ticket ticket) => ticket.Grant("standardni ulaz");
    }

    public class TicketingSystem
    {
        private readonly ITicketProcessor pocetak;

        public TicketingSystem(ISet<string> invalidCodes)
        {
            ITicketProcessor[] karike =
            {
                new CounterfeitProcessor(invalidCodes),
                new VipProcessor(),
                new StandardProcessor(),
            };

            for (int i = 0; i < karike.Length - 1; i++)
            {
                karike[i].SetNext(karike[i + 1]);
            }

            this.pocetak = karike[0];
        }

        public void Admit(Ticket ticket) => this.pocetak.Process(ticket);
    }
}
