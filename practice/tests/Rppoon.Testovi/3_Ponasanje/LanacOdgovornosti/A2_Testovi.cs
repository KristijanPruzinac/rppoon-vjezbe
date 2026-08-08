using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.LanacOdgovornosti.A2;

namespace Rppoon.Testovi.LanacOdgovornosti
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("LanacOdgovornosti")]
    [Category("A")]
    public class A2_Testovi
    {
        private static MailProcessor Lanac(params MailProcessor[] karike)
        {
            for (int i = 0; i < karike.Length - 1; i++)
            {
                karike[i].SetNext(karike[i + 1]);
            }
            return karike[0];
        }

        [Test]
        public void SpamJeZabiljezenALIJEIDALJEArhiviran()
        {
            MailProcessor prvi = Lanac(new SpamProcessor("kredit"), new ArchiveProcessor());
            Mail poruka = new Mail("ana@example.com", "povoljan kredit odmah");

            prvi.Handle(poruka);

            Assert.That(poruka.Log, Is.EqualTo(new[] { "spam:ana@example.com", "arhiva" }),
                "Necisti lanac: karika odradi svoje pa SVEJEDNO proslijedi dalje.");
        }

        [Test]
        public void CistaPoruka_SamoArhiva()
        {
            MailProcessor prvi = Lanac(new SpamProcessor("kredit"), new ArchiveProcessor());
            Mail poruka = new Mail("ivo@example.com", "vidimo se u petak");

            prvi.Handle(poruka);

            Assert.That(poruka.Log, Is.EqualTo(new[] { "arhiva" }));
        }

        [Test]
        public void SvaTriObradivacaVidePorukuRedom()
        {
            MailProcessor prvi = Lanac(
                new SpamProcessor("kredit"),
                new ComplaintProcessor(),
                new ArchiveProcessor());
            Mail poruka = new Mail("ana@example.com", "kredit i reklamacija u istoj poruci");

            prvi.Handle(poruka);

            Assert.That(poruka.Log, Is.EqualTo(new[] { "spam:ana@example.com", "prigovor", "arhiva" }));
        }

        [Test]
        public void ZadnjaKarika_BezSljedecega_NePuca()
        {
            ArchiveProcessor sama = new ArchiveProcessor();
            Mail poruka = new Mail("ivo@example.com", "bilo sto");

            sama.Handle(poruka);

            Assert.That(poruka.Log, Is.EqualTo(new[] { "arhiva" }));
        }

        [Test]
        public void SetNext_PrevezivanjeMijenjaOstatakLanca()
        {
            SpamProcessor prvi = new SpamProcessor("kredit");
            prvi.SetNext(new ComplaintProcessor());
            prvi.SetNext(new ArchiveProcessor());

            Mail poruka = new Mail("ana@example.com", "reklamacija na kredit");
            prvi.Handle(poruka);

            Assert.That(poruka.Log, Is.EqualTo(new[] { "spam:ana@example.com", "arhiva" }),
                "SetNext postavlja sljedecu kariku, ne dodaje jos jednu.");
        }

        [Test]
        public void IstiObradivacPonovoUpotrijebljenZaDruguPoruku()
        {
            MailProcessor prvi = Lanac(new SpamProcessor("kredit"), new ArchiveProcessor());

            Mail prva = new Mail("ana@example.com", "kredit");
            Mail druga = new Mail("ivo@example.com", "pozdrav");
            prvi.Handle(prva);
            prvi.Handle(druga);

            Assert.That(prva.Log, Is.EqualTo(new[] { "spam:ana@example.com", "arhiva" }));
            Assert.That(druga.Log, Is.EqualTo(new[] { "arhiva" }),
                "Lanac ne smije pamtiti nista o prethodnoj poruci.");
        }

        [Test]
        public void Gradja_ObradivacDrziReferencuNaSljedecegObradivaca()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(MailProcessor), typeof(MailProcessor)), Is.True,
                "Karika lanca mora znati sljedecu kariku - i to kao MailProcessor.");
        }

        [Test]
        public void Gradja_KonkretniObradivaciSuKarikeIstogLanca()
        {
            Assert.That(Odraz.Nasljednici(typeof(MailProcessor)),
                Has.Count.GreaterThanOrEqualTo(3));
        }

        [Test]
        public void Gradja_ObradivacNeZnaZaKonkretnogSljedecega()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(SpamProcessor), typeof(ArchiveProcessor)), Is.False);
            Assert.That(Odraz.DrziClanTipa(typeof(ComplaintProcessor), typeof(ArchiveProcessor)), Is.False);
        }
    }
}
