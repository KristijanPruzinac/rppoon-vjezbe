using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.B1
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti B1 =====
    //
    // Limit je apstraktno svojstvo, a odlucivanje je jednom napisano u
    // bazi (Predlozak metode unutar Lanca). Naziv se cita iz tipa, a ne
    // upisuje kao doslovni niz. Isto ponasanje, drugacija podjela koda.

    public class PurchaseRequest
    {
        private readonly List<string> seen = new List<string>();

        public PurchaseRequest(decimal amount, string purpose)
        {
            this.Amount = amount;
            this.Purpose = purpose;
        }

        public decimal Amount { get; private set; }

        public string Purpose { get; private set; }

        public IList<string> Seen => this.seen;
    }

    public abstract class Approver
    {
        protected Approver next;

        public void SetNext(Approver approver)
        {
            this.next = approver;
        }

        protected abstract decimal Limit { get; }

        public virtual string Approve(PurchaseRequest request)
        {
            request.Seen.Add(this.GetType().Name);

            bool preveliko = request.Amount > this.Limit;
            if (!preveliko)
            {
                return this.GetType().Name;
            }

            if (this.next == null)
            {
                return null;
            }

            return this.next.Approve(request);
        }
    }

    public class TeamLead : Approver
    {
        protected override decimal Limit => 1000m;
    }

    public class Manager : Approver
    {
        protected override decimal Limit => 10000m;
    }

    public class Director : Approver
    {
        protected override decimal Limit => 100000m;
    }
}
