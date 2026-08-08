using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.LanacOdgovornosti.B1;

namespace Rppoon.Testovi.LanacOdgovornosti
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("LanacOdgovornosti")]
    [Category("B")]
    public class B1_Testovi
    {
        private static Approver Hijerarhija(params Approver[] razine)
        {
            for (int i = 0; i < razine.Length - 1; i++)
            {
                razine[i].SetNext(razine[i + 1]);
            }
            return razine[0];
        }

        private static Approver PunaHijerarhija()
        {
            return Hijerarhija(new TeamLead(), new Manager(), new Director());
        }

        [Test]
        public void MaliIznos_OdobravaPrvaRazina()
        {
            PurchaseRequest zahtjev = new PurchaseRequest(500m, "monitor");

            string odobrio = PunaHijerarhija().Approve(zahtjev);

            Assert.That(odobrio, Is.EqualTo("TeamLead"));
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "TeamLead" }),
                "Cisti lanac: cim netko odobri, dalje se NE ide.");
        }

        [Test]
        public void SrednjiIznos_PenjeSeJednuRazinu()
        {
            PurchaseRequest zahtjev = new PurchaseRequest(5000m, "prijenosnik");

            string odobrio = PunaHijerarhija().Approve(zahtjev);

            Assert.That(odobrio, Is.EqualTo("Manager"));
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "TeamLead", "Manager" }));
        }

        [Test]
        public void VelikIznos_DolaziDoDirektora()
        {
            PurchaseRequest zahtjev = new PurchaseRequest(50000m, "posluzitelj");

            string odobrio = PunaHijerarhija().Approve(zahtjev);

            Assert.That(odobrio, Is.EqualTo("Director"));
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "TeamLead", "Manager", "Director" }));
        }

        [Test]
        public void PrevelikIznos_NeOdobravaNitko()
        {
            PurchaseRequest zahtjev = new PurchaseRequest(500000m, "zgrada");

            string odobrio = PunaHijerarhija().Approve(zahtjev);

            Assert.That(odobrio, Is.Null,
                "Zahtjev je prosao cijeli lanac i nitko ga nije mogao odobriti.");
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "TeamLead", "Manager", "Director" }));
        }

        [Test]
        public void GranicniIznos_JosUvijekStaneURazinu()
        {
            PurchaseRequest naGranici = new PurchaseRequest(1000m, "stolica");
            PurchaseRequest tikIznad = new PurchaseRequest(1000.01m, "stolica");

            Assert.That(PunaHijerarhija().Approve(naGranici), Is.EqualTo("TeamLead"),
                "Limit je ukljucen: 'do 1000' znaci i tocno 1000.");
            Assert.That(PunaHijerarhija().Approve(tikIznad), Is.EqualTo("Manager"));
        }

        [Test]
        public void LanacSeMozePresloziti()
        {
            // Nijedna razina ne smije pretpostavljati tko je iza nje.
            PurchaseRequest zahtjev = new PurchaseRequest(500m, "monitor");

            string odobrio = Hijerarhija(new Director(), new Manager(), new TeamLead()).Approve(zahtjev);

            Assert.That(odobrio, Is.EqualTo("Director"),
                "Sada je direktor prvi, a 500 stane u njegov limit.");
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "Director" }));
        }

        [Test]
        public void KracaHijerarhija_BezDirektora()
        {
            PurchaseRequest zahtjev = new PurchaseRequest(50000m, "posluzitelj");

            string odobrio = Hijerarhija(new TeamLead(), new Manager()).Approve(zahtjev);

            Assert.That(odobrio, Is.Null,
                "Razine se moraju moci izbaciti bez izmjene ostalih razreda.");
            Assert.That(zahtjev.Seen, Is.EqualTo(new[] { "TeamLead", "Manager" }));
        }

        [Test]
        public void SamaRazinaBezSljedece()
        {
            PurchaseRequest mali = new PurchaseRequest(100m, "mis");
            PurchaseRequest velik = new PurchaseRequest(100000m, "vozilo");

            Assert.That(new TeamLead().Approve(mali), Is.EqualTo("TeamLead"));
            Assert.That(new TeamLead().Approve(velik), Is.Null);
        }

        [Test]
        public void Gradja_SveRazineSuKarikeIstogLanca()
        {
            Assert.That(typeof(Approver).IsAssignableFrom(typeof(TeamLead)), Is.True);
            Assert.That(typeof(Approver).IsAssignableFrom(typeof(Manager)), Is.True);
            Assert.That(typeof(Approver).IsAssignableFrom(typeof(Director)), Is.True);
        }

        [Test]
        public void Gradja_RazinaNeZnaZaKonkretnuSljedecuRazinu()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(TeamLead), typeof(Manager)), Is.False,
                "Da TeamLead drzi polje tipa Manager, hijerarhija bi bila zabetonirana.");
            Assert.That(Odraz.DrziClanTipa(typeof(Manager), typeof(Director)), Is.False);
        }
    }
}
