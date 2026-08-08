using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.A2
{
    // ===== ALTERNATIVNO RJESENJE - Lanac odgovornosti A2 =====
    //
    // Handle ne poziva samog sebe rekurzivno nego HODA lancem u petlji.
    // Ponasanje je isto: svaka karika dobije poruku, redom.

    public class Mail
    {
        private readonly List<string> log = new List<string>();

        public Mail(string sender, string contents)
        {
            this.Sender = sender;
            this.Contents = contents;
        }

        public string Sender { get; private set; }

        public string Contents { get; private set; }

        public IList<string> Log => this.log;
    }

    public abstract class MailProcessor
    {
        protected MailProcessor next;

        public void SetNext(MailProcessor mailProcessor)
        {
            this.next = mailProcessor;
        }

        public void Handle(Mail mail)
        {
            MailProcessor trenutni = this;
            while (trenutni != null)
            {
                trenutni.Process(mail);
                trenutni = trenutni.next;
            }
        }

        protected abstract void Process(Mail mail);
    }

    public class SpamProcessor : MailProcessor
    {
        private readonly string term;

        public SpamProcessor(string term)
        {
            this.term = term;
        }

        protected override void Process(Mail mail)
        {
            if (mail.Contents.IndexOf(this.term) >= 0)
            {
                mail.Log.Add("spam:" + mail.Sender);
            }
        }
    }

    public class ComplaintProcessor : MailProcessor
    {
        protected override void Process(Mail mail)
        {
            if (mail.Contents.IndexOf("reklamacija") >= 0)
            {
                mail.Log.Add("prigovor");
            }
        }
    }

    public class ArchiveProcessor : MailProcessor
    {
        protected override void Process(Mail mail)
        {
            mail.Log.Add("arhiva");
        }
    }
}
