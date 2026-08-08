using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.B2
{
    // ================================================================
    //  LANAC ODGOVORNOSTI - razina B (sastavi), zadatak 2: Ulaznice
    // ================================================================
    //  Ovdje karika NIJE apstraktni razred nego SUCELJE. Lanac ne ovisi
    //  o nasljedivanju - ovisi samo o tome da svaka karika zna sljedecu
    //  i da ih klijent moze nanizati.
    //
    //  Novo u odnosu na A1/A2: postoji i KLIJENT (TicketingSystem). On
    //  slaze lanac u konstruktoru i predaje mu ulaznicu. Nakon toga vise
    //  ne zna tko je sto obradio - drzi samo prvu kariku.
    //
    //  Lanac (tim redom):  CounterfeitProcessor -> VipProcessor -> StandardProcessor
    //
    //  CounterfeitProcessor  kod je na popisu nevazecih -> Reject("krivotvorina"), STANI
    //                        inace proslijedi
    //  VipProcessor          ulaznica je VIP -> Grant("vip ulaz"), STANI
    //                        inace proslijedi
    //  StandardProcessor     Grant("standardni ulaz"), zadnji je u lancu
    //
    //  Ovo je CISTI lanac: cim netko odluci, dalje se ne ide.
    // ================================================================

    /// <summary>DANO: svaka odluka se biljezi; Status je zadnja donesena.</summary>
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

    /// <summary>DANO: obradivac. Nema baznog razreda - samo dogovor.</summary>
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

        public void SetNext(ITicketProcessor processor)
        {
            // TODO
            throw new NotImplementedException("CounterfeitProcessor.SetNext");
        }

        public void Process(Ticket ticket)
        {
            // TODO: nevazeci kod -> ticket.Reject("krivotvorina") i STANI.
            //       inace proslijedi sljedecem, ako ga ima.
            throw new NotImplementedException("CounterfeitProcessor.Process");
        }
    }

    public class VipProcessor : ITicketProcessor
    {
        public void SetNext(ITicketProcessor processor)
        {
            // TODO
            throw new NotImplementedException("VipProcessor.SetNext");
        }

        public void Process(Ticket ticket)
        {
            // TODO: VIP -> ticket.Grant("vip ulaz") i STANI, inace proslijedi.
            throw new NotImplementedException("VipProcessor.Process");
        }
    }

    public class StandardProcessor : ITicketProcessor
    {
        public void SetNext(ITicketProcessor processor)
        {
            // TODO
            throw new NotImplementedException("StandardProcessor.SetNext");
        }

        public void Process(Ticket ticket)
        {
            // TODO: uvijek ticket.Grant("standardni ulaz").
            throw new NotImplementedException("StandardProcessor.Process");
        }
    }

    /// <summary>Klijent: slozi lanac i predaj mu ulaznicu.</summary>
    public class TicketingSystem
    {
        public TicketingSystem(ISet<string> invalidCodes)
        {
            // TODO
            //  1. stvori tri obradivaca
            //  2. nanizi ih: krivotvorine -> vip -> standardni
            //  3. zapamti SAMO prvu kariku, i to kao ITicketProcessor
            //     (klijent ne smije drzati polje konkretnog tipa)
            throw new NotImplementedException("TicketingSystem - konstruktor");
        }

        public void Admit(Ticket ticket)
        {
            // TODO: predaj ulaznicu prvoj karici.
            throw new NotImplementedException("TicketingSystem.Admit");
        }
    }
}
