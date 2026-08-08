using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.B1
{
    // ================================================================
    //  LANAC ODGOVORNOSTI - razina B (sastavi), zadatak 1: Odobravanje
    // ================================================================
    //  Zahtjev za nabavu putuje hijerarhijom dok ga netko ne odobri.
    //  Svaka razina ima svoj limit; PRVI kome iznos stane u limit
    //  odobrava i lanac tu STAJE.
    //
    //  Ovo je CISTI lanac - tocno jedan obradivac obavi posao. Usporedi
    //  sa zadacima A1 i A2, gdje poruka prolazi kroz sve karike.
    //
    //  Limiti:  TeamLead do 1000, Manager do 10000, Director do 100000.
    //  Iznad toga ne odobrava nitko.
    //
    //  Approve vraca naziv onoga tko je odobrio, ili null ako nitko.
    //  Svaki obradivac na POCETKU svog Approve upise naziv svog razreda
    //  u request.Seen - tako se vidi dokle je zahtjev doputovao.
    // ================================================================

    /// <summary>DANO: zahtjev za nabavu.</summary>
    public class PurchaseRequest
    {
        public PurchaseRequest(decimal amount, string purpose)
        {
            this.Amount = amount;
            this.Purpose = purpose;
        }

        public decimal Amount { get; private set; }

        public string Purpose { get; private set; }

        /// <summary>Tko je zahtjev uopce vidio, redom.</summary>
        public IList<string> Seen { get; } = new List<string>();
    }

    public abstract class Approver
    {
        protected Approver next;

        public void SetNext(Approver approver)
        {
            // TODO
            throw new NotImplementedException("Approver.SetNext");
        }

        /// <summary>Naziv odobravatelja, ili null ako ga nitko u lancu nije odobrio.</summary>
        public abstract string Approve(PurchaseRequest request);
    }

    public class TeamLead : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            // TODO
            //  1. request.Seen.Add("TeamLead")
            //  2. ako je iznos <= 1000 -> vrati "TeamLead" i NE idi dalje
            //  3. inace proslijedi sljedecem; ako sljedecega nema -> null
            throw new NotImplementedException("TeamLead.Approve");
        }
    }

    public class Manager : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            // TODO: isti oblik, limit 10000, naziv "Manager".
            throw new NotImplementedException("Manager.Approve");
        }
    }

    public class Director : Approver
    {
        public override string Approve(PurchaseRequest request)
        {
            // TODO: isti oblik, limit 100000, naziv "Director".
            throw new NotImplementedException("Director.Approve");
        }
    }
}
