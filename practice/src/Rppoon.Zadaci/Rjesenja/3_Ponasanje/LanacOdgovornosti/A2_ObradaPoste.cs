using System.Collections.Generic;

namespace Rppoon.Zadaci.LanacOdgovornosti.A2
{
    // ======= REFERENTNO RJESENJE - Lanac odgovornosti A2 =======

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
            this.next = mailProcessor;
        }

        public void Handle(Mail mail)
        {
            this.Process(mail);

            if (this.next != null)
            {
                this.next.Handle(mail);
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
            if (mail.Contents.Contains(this.term))
            {
                mail.Log.Add("spam:" + mail.Sender);
            }
        }
    }

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

    public class ArchiveProcessor : MailProcessor
    {
        protected override void Process(Mail mail)
        {
            mail.Log.Add("arhiva");
        }
    }
}
