using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.B1
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti B1 =======

    public class PurchaseRequest
    {
        public PurchaseRequest(decimal amount, string purpose)
        {
            this.Amount = amount;
            this.Purpose = purpose;
        }

        public decimal Amount { get; private set; }

        public string Purpose { get; private set; }

        public IList<string> Seen { get; } = new List<string>();
    }

    public abstract class Approver
    {
        protected Approver next;

        public void SetNext(Approver approver)
        {
            this.next = approver;
        }

        public abstract string Approve(PurchaseRequest request);

        /// <summary>Ostatak lanca, ili null ako smo na kraju.</summary>
        protected string Proslijedi(PurchaseRequest request)
        {
            return this.next == null ? null : this.next.Approve(request);
        }
    }

    public class TeamLead : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            request.Seen.Add("TeamLead");

            if (request.Amount <= 1000m)
            {
                return "TeamLead";
            }

            return this.Proslijedi(request);
        }
    }

    public class Manager : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            request.Seen.Add("Manager");

            if (request.Amount <= 10000m)
            {
                return "Manager";
            }

            return this.Proslijedi(request);
        }
    }

    public class Director : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            request.Seen.Add("Director");

            if (request.Amount <= 100000m)
            {
                return "Director";
            }

            return this.Proslijedi(request);
        }
    }
}
