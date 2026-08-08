using System.Collections.Generic;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.LanacOdgovornosti.B2;

namespace Rppoon.Testovi.LanacOdgovornosti
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("LanacOdgovornosti")]
    [Category("B")]
    public class B2_Testovi
    {
        private static ISet<string> Nevazeci(params string[] kodovi)
        {
            return new HashSet<string>(kodovi);
        }

        [Test]
        public void ObicnaUlaznica_StandardniUlaz()
        {
            TicketingSystem sustav = new TicketingSystem(Nevazeci());
            Ticket ulaznica = new Ticket("A-100", false);

            sustav.Admit(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("standardni ulaz"));
        }

        [Test]
        public void VipUlaznica_VipUlaz()
        {
            TicketingSystem sustav = new TicketingSystem(Nevazeci());
            Ticket ulaznica = new Ticket("A-200", true);

            sustav.Admit(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("vip ulaz"));
        }

        [Test]
        public void KrivotvorenaUlaznica_Odbijena()
        {
            TicketingSystem sustav = new TicketingSystem(Nevazeci("X-999"));
            Ticket ulaznica = new Ticket("X-999", false);

            sustav.Admit(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("krivotvorina"));
        }

        [Test]
        public void SvakaUlaznicaDobivaTOCNOJEDNUOdluku()
        {
            TicketingSystem sustav = new TicketingSystem(Nevazeci("X-999"));
            Ticket krivotvorena = new Ticket("X-999", true);
            Ticket vip = new Ticket("A-200", true);
            Ticket obicna = new Ticket("A-300", false);

            sustav.Admit(krivotvorena);
            sustav.Admit(vip);
            sustav.Admit(obicna);

            Assert.That(krivotvorena.Decisions, Has.Count.EqualTo(1),
                "Cisti lanac: cim obradivac odluci, ulaznica ne ide dalje.");
            Assert.That(vip.Decisions, Has.Count.EqualTo(1));
            Assert.That(obicna.Decisions, Has.Count.EqualTo(1));
        }

        [Test]
        public void KrivotvorenaVipUlaznica_RedoslijedULancuOdlucuje()
        {
            TicketingSystem sustav = new TicketingSystem(Nevazeci("X-999"));
            Ticket ulaznica = new Ticket("X-999", true);

            sustav.Admit(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("krivotvorina"),
                "Provjera krivotvorina je prva u lancu, pa VIP status vise nije bitan.");
        }

        [Test]
        public void ObradivaciSeMoguKoristitiIIzvanSustava()
        {
            // Isti obradivaci, rucno slozen kraci lanac bez provjere krivotvorina.
            VipProcessor vip = new VipProcessor();
            vip.SetNext(new StandardProcessor());

            Ticket ulaznica = new Ticket("X-999", false);
            vip.Process(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("standardni ulaz"),
                "Karike ne smiju ovisiti o tome tko ih je slozio.");
        }

        [Test]
        public void ZadnjaKarika_BezSljedeceNePuca()
        {
            StandardProcessor sam = new StandardProcessor();
            Ticket ulaznica = new Ticket("A-400", false);

            sam.Process(ulaznica);

            Assert.That(ulaznica.Status, Is.EqualTo("standardni ulaz"));
        }

        [Test]
        public void Gradja_SviObradivaciDijeleSucelje()
        {
            Assert.That(typeof(ITicketProcessor).IsAssignableFrom(typeof(CounterfeitProcessor)), Is.True);
            Assert.That(typeof(ITicketProcessor).IsAssignableFrom(typeof(VipProcessor)), Is.True);
            Assert.That(typeof(ITicketProcessor).IsAssignableFrom(typeof(StandardProcessor)), Is.True);
        }

        [Test]
        public void Gradja_KlijentDrziSamoPrvuKarikuIToApstraktno()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(TicketingSystem), typeof(ITicketProcessor)), Is.True,
                "Klijent mora drzati pocetak lanca.");

            Assert.That(Odraz.DrziClanTipa(typeof(TicketingSystem), typeof(CounterfeitProcessor)), Is.False,
                "Polje konkretnog tipa znaci da klijent zna tko obraduje - to obrazac upravo skriva.");
            Assert.That(Odraz.DrziClanTipa(typeof(TicketingSystem), typeof(VipProcessor)), Is.False);
            Assert.That(Odraz.DrziClanTipa(typeof(TicketingSystem), typeof(StandardProcessor)), Is.False);
        }

        [Test]
        public void Gradja_ObradivacNeZnaZaKonkretnogSljedecega()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(CounterfeitProcessor), typeof(VipProcessor)), Is.False);
            Assert.That(Odraz.DrziClanTipa(typeof(VipProcessor), typeof(StandardProcessor)), Is.False);
        }
    }
}
