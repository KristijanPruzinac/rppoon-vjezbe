using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.LanacOdgovornosti
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("LanacOdgovornosti")]
    [Category("C")]
    public class C1_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.LanacOdgovornosti.C1";

        private static object Prijava(string naslov, int ozbiljnost)
            => Odraz.Novi(Odraz.Tip(Ns, "SupportTicket"), naslov, ozbiljnost);

        private static object Razina(string naziv)
            => Odraz.Novi(Odraz.Tip(Ns, naziv));

        /// <summary>Nanize obradivace i vrati prvoga.</summary>
        private static object Lanac(params object[] karike)
        {
            for (int i = 0; i < karike.Length - 1; i++)
            {
                Odraz.Pozovi(karike[i], "SetNext", karike[i + 1]);
            }
            return karike[0];
        }

        private static object PunLanac()
            => Lanac(Razina("Level1Support"), Razina("Level2Support"), Razina("EngineeringTeam"));

        private static string[] Trag(object prijava)
        {
            object vrijednost = Odraz.Pozovi(prijava, "Trail");
            Assert.That(vrijednost, Is.Not.Null, "SupportTicket.Trail ne smije biti null.");

            IEnumerable zbirka = vrijednost as IEnumerable;
            Assert.That(zbirka, Is.Not.Null, "SupportTicket.Trail mora biti zbirka nizova znakova.");

            return zbirka.Cast<object>().Select(s => Convert.ToString(s)).ToArray();
        }

        [Test]
        public void NiskaOzbiljnost_RjesavaPrvaRazina()
        {
            object prijava = Prijava("zaboravljena lozinka", 1);

            object rijesio = Odraz.Pozovi(PunLanac(), "Handle", prijava);

            Assert.That(rijesio, Is.EqualTo("Level1Support"));
            Assert.That(Trag(prijava), Is.EqualTo(new[] { "Level1Support" }),
                "Cisti lanac: rijesenu prijavu se ne prosljeduje dalje.");
        }

        [Test]
        public void SrednjaOzbiljnost_PenjeSeNaDruguRazinu()
        {
            object prijava = Prijava("aplikacija se rusi", 3);

            object rijesio = Odraz.Pozovi(PunLanac(), "Handle", prijava);

            Assert.That(rijesio, Is.EqualTo("Level2Support"));
            Assert.That(Trag(prijava), Is.EqualTo(new[] { "Level1Support", "Level2Support" }));
        }

        [Test]
        public void VisokaOzbiljnost_DolaziDoInzenjera()
        {
            object prijava = Prijava("gubitak podataka", 9);

            object rijesio = Odraz.Pozovi(PunLanac(), "Handle", prijava);

            Assert.That(rijesio, Is.EqualTo("EngineeringTeam"));
            Assert.That(Trag(prijava), Is.EqualTo(new[] { "Level1Support", "Level2Support", "EngineeringTeam" }));
        }

        [Test]
        public void GranicaRazine_JosUvijekPripadaTojRazini()
        {
            object naGranici = Prijava("sitnica", 1);
            object tikIznad = Prijava("sitnica", 2);

            Assert.That(Odraz.Pozovi(PunLanac(), "Handle", naGranici), Is.EqualTo("Level1Support"),
                "'Severity <= 1' ukljucuje i tocno 1.");
            Assert.That(Odraz.Pozovi(PunLanac(), "Handle", tikIznad), Is.EqualTo("Level2Support"));
        }

        [Test]
        public void NepotpunLanac_MozeOstatiNerijeseno()
        {
            object prijava = Prijava("gubitak podataka", 9);

            object rijesio = Odraz.Pozovi(Razina("Level1Support"), "Handle", prijava);

            Assert.That(rijesio, Is.Null,
                "Nitko u lancu nije bio nadlezan - rezultat je null, ne iznimka.");
            Assert.That(Trag(prijava), Is.EqualTo(new[] { "Level1Support" }));
        }

        [Test]
        public void LanacSeMozePresloziti()
        {
            object prijava = Prijava("zaboravljena lozinka", 1);

            object rijesio = Odraz.Pozovi(
                Lanac(Razina("Level2Support"), Razina("Level1Support")), "Handle", prijava);

            Assert.That(rijesio, Is.EqualTo("Level2Support"),
                "Prva razina u lancu je sada druga linija podrske, a ona pokriva i ozbiljnost 1.");
            Assert.That(Trag(prijava), Is.EqualTo(new[] { "Level2Support" }));
        }

        [Test]
        public void IstiLanacRjesavaViseUzastopnihPrijava()
        {
            object lanac = PunLanac();
            object prva = Prijava("sitnica", 1);
            object druga = Prijava("gubitak podataka", 9);

            Odraz.Pozovi(lanac, "Handle", prva);
            Odraz.Pozovi(lanac, "Handle", druga);

            Assert.That(Trag(prva), Is.EqualTo(new[] { "Level1Support" }));
            Assert.That(Trag(druga), Is.EqualTo(new[] { "Level1Support", "Level2Support", "EngineeringTeam" }),
                "Lanac ne smije pamtiti nista o prethodnoj prijavi.");
        }

        [Test]
        public void PrazanTragPrijeObrade()
        {
            Assert.That(Trag(Prijava("nova prijava", 2)), Is.Empty);
        }

        [Test]
        public void Gradja_SveTriRazineSuKarikeIstogLanca()
        {
            Type baza = Odraz.Tip(Ns, "SupportHandler");
            Assert.That(baza.IsAbstract, Is.True, "SupportHandler mora biti apstraktan.");

            IEnumerable<string> nasljednici = Odraz.Nasljednici(baza).Select(t => t.Name);
            Assert.That(nasljednici, Is.SupersetOf(
                new[] { "Level1Support", "Level2Support", "EngineeringTeam" }));
        }

        [Test]
        public void Gradja_ObradivacDrziReferencuNaSljedecegObradivaca()
        {
            Type baza = Odraz.Tip(Ns, "SupportHandler");

            Assert.That(Odraz.DrziClanTipa(baza, baza), Is.True,
                "SupportHandler mora drzati sljedeceg obradivaca, i to kao SupportHandler.");
        }

        [Test]
        public void Gradja_RazinaNeZnaZaKonkretnuSljedecuRazinu()
        {
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "Level1Support"), Odraz.Tip(Ns, "Level2Support")),
                Is.False,
                "Da prva razina drzi polje tipa Level2Support, lanac se ne bi mogao presloziti.");
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "Level2Support"), Odraz.Tip(Ns, "EngineeringTeam")),
                Is.False);
        }
    }
}
