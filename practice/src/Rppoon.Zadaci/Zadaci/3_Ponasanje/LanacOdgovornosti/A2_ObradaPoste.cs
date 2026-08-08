using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.A2
{
    // ================================================================
    //  LANAC ODGOVORNOSTI - razina A (vodeno), zadatak 2: Obrada poste
    // ================================================================
    //  Obrnuto od zadatka A1: konkretni obradivaci su DANI, a ti pises
    //  KARIKU - zajednicki dio koji drzi vezu na sljedecega i vodi
    //  putovanje poruke.
    //
    //  Ovo je smisao lanca: svaki obradivac zna samo svoj jedan posao
    //  ('Process'), a nista o tome tko je iza njega. Vezanje i
    //  prosljedivanje je odgovornost baze - to je "single responsibility"
    //  u ovom obrascu.
    //
    //  Lanac je NECISTI: obradivac odradi svoje pa SVEJEDNO proslijedi
    //  dalje. Poruka prolazi kroz sve karike.
    // ================================================================

    /// <summary>DANO: poruka koja putuje lancem; 'Log' biljezi sto se dogodilo.</summary>
    public class Mail
    {
        public Mail(string sender, string contents)
        {
            this.Sender = sender;
            this.Contents = contents;
        }

        public string Sender { get; private set; }

        public string Contents { get; private set; }

        public IList<string> Log { get; } = new List<string>();
    }

    public abstract class MailProcessor
    {
        protected MailProcessor next;

        public void SetNext(MailProcessor mailProcessor)
        {
            // TODO: zapamti sljedecu kariku lanca.
            throw new NotImplementedException("MailProcessor.SetNext");
        }

        /// <summary>Ulazna tocka: obradi poruku pa je pusti dalje.</summary>
        public void Handle(Mail mail)
        {
            // TODO
            //  1. odradi svoj dio posla - pozovi Process(mail)
            //  2. ako sljedeca karika postoji, predaj joj poruku
            //  Napomena: prosljeduje se UVIJEK, i onda kad je ova karika
            //  nesto zabiljezila. To je necisti lanac.
            throw new NotImplementedException("MailProcessor.Handle");
        }

        /// <summary>Jedini posao konkretnog obradivaca.</summary>
        protected abstract void Process(Mail mail);
    }

    /// <summary>DAN: biljezi posiljatelja ako poruka sadrzi sumnjiv izraz.</summary>
    public class SpamProcessor : MailProcessor
    {
        private readonly string term;

        public SpamProcessor(string term)
        {
            this.term = term;
        }

        protected override void Process(Mail mail)
        {
            if (mail.Contents.Contains(this.term))
            {
                mail.Log.Add("spam:" + mail.Sender);
            }
        }
    }

    /// <summary>DAN: prepoznaje prigovor i prijavljuje ga sluzbi.</summary>
    public class ComplaintProcessor : MailProcessor
    {
        protected override void Process(Mail mail)
        {
            if (mail.Contents.Contains("reklamacija"))
            {
                mail.Log.Add("prigovor");
            }
        }
    }

    /// <summary>DAN: sprema svaku poruku, bez iznimke.</summary>
    public class ArchiveProcessor : MailProcessor
    {
        protected override void Process(Mail mail)
        {
            mail.Log.Add("arhiva");
        }
    }
}
