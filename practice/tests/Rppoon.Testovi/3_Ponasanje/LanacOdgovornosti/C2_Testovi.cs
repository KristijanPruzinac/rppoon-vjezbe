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
    public class C2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.LanacOdgovornosti.C2";

        private static object Prijava(string korisnik, string lozinka, int dob)
            => Odraz.Novi(Odraz.Tip(Ns, "Registration"), korisnik, lozinka, dob);

        private static object Pravilo(string naziv)
            => Odraz.Novi(Odraz.Tip(Ns, naziv));

        private static object Lanac(params object[] pravila)
        {
            for (int i = 0; i < pravila.Length - 1; i++)
            {
                Odraz.Pozovi(pravila[i], "SetNext", pravila[i + 1]);
            }
            return pravila[0];
        }

        private static object SvaPravila()
            => Lanac(Pravilo("UsernameValidator"), Pravilo("PasswordValidator"), Pravilo("AgeValidator"));

        private static string[] Pogreske(object lanac, object prijava)
        {
            object vrijednost = Odraz.Pozovi(lanac, "Validate", prijava);
            Assert.That(vrijednost, Is.Not.Null,
                "Validate mora vratiti popis pogresaka - prazan popis, ne null.");

            IEnumerable zbirka = vrijednost as IEnumerable;
            Assert.That(zbirka, Is.Not.Null, "Validate mora vratiti zbirku nizova znakova.");

            return zbirka.Cast<object>().Select(p => Convert.ToString(p)).ToArray();
        }

        [Test]
        public void IspravnaPrijava_NemaPogresaka()
        {
            string[] pogreske = Pogreske(SvaPravila(), Prijava("kristijan", "duga-lozinka", 21));

            Assert.That(pogreske, Is.Empty);
        }

        [Test]
        public void SvePogresno_PrijavljujuSeSVEPogreskeOdjednom()
        {
            string[] pogreske = Pogreske(SvaPravila(), Prijava("ab", "kratka", 15));

            Assert.That(pogreske, Is.EqualTo(new[] { "korisnicko ime", "lozinka", "dob" }),
                "Necisti lanac: svako pravilo doda svoje pa SVEJEDNO proslijedi dalje. " +
                "Da se lanac prekidao na prvoj pogresci, ovdje bi bila samo jedna.");
        }

        [Test]
        public void SamoLozinkaLosa()
        {
            string[] pogreske = Pogreske(SvaPravila(), Prijava("kristijan", "kratka", 21));

            Assert.That(pogreske, Is.EqualTo(new[] { "lozinka" }));
        }

        [Test]
        public void SamoDobLosa()
        {
            string[] pogreske = Pogreske(SvaPravila(), Prijava("kristijan", "duga-lozinka", 17));

            Assert.That(pogreske, Is.EqualTo(new[] { "dob" }));
        }

        [Test]
        public void GranicneVrijednosti()
        {
            Assert.That(Pogreske(SvaPravila(), Prijava("abc", "osamzna", 18)),
                Is.EqualTo(new[] { "lozinka" }),
                "Ime od 3 znaka i dob 18 su u redu; lozinka od 7 znakova nije.");

            Assert.That(Pogreske(SvaPravila(), Prijava("abc", "osamznak", 18)), Is.Empty);
        }

        [Test]
        public void PrazniPodaci_NePucaNegoPrijavljujePogresku()
        {
            string[] pogreske = Pogreske(SvaPravila(), Prijava(null, null, 0));

            Assert.That(pogreske, Is.EqualTo(new[] { "korisnicko ime", "lozinka", "dob" }));
        }

        [Test]
        public void RedoslijedPogresakaPratiRedoslijedULancu()
        {
            object obrnuto = Lanac(
                Pravilo("AgeValidator"), Pravilo("PasswordValidator"), Pravilo("UsernameValidator"));

            string[] pogreske = Pogreske(obrnuto, Prijava("ab", "kratka", 15));

            Assert.That(pogreske, Is.EqualTo(new[] { "dob", "lozinka", "korisnicko ime" }));
        }

        [Test]
        public void PraviloSeMozeIzbaciti()
        {
            object bezDobi = Lanac(Pravilo("UsernameValidator"), Pravilo("PasswordValidator"));

            string[] pogreske = Pogreske(bezDobi, Prijava("ab", "kratka", 15));

            Assert.That(pogreske, Is.EqualTo(new[] { "korisnicko ime", "lozinka" }),
                "Pravila se moraju moci micati bez izmjene ostalih razreda.");
        }

        [Test]
        public void JednoSamoPravilo_BezSljedecegNePuca()
        {
            Assert.That(Pogreske(Pravilo("AgeValidator"), Prijava("kristijan", "duga-lozinka", 15)),
                Is.EqualTo(new[] { "dob" }));
            Assert.That(Pogreske(Pravilo("AgeValidator"), Prijava("kristijan", "duga-lozinka", 30)),
                Is.Empty);
        }

        [Test]
        public void IstiLanacProvjeravaViseUzastopnihPrijava()
        {
            object lanac = SvaPravila();

            Assert.That(Pogreske(lanac, Prijava("ab", "kratka", 15)),
                Is.EqualTo(new[] { "korisnicko ime", "lozinka", "dob" }));
            Assert.That(Pogreske(lanac, Prijava("kristijan", "duga-lozinka", 21)), Is.Empty,
                "Lanac ne smije pamtiti pogreske prethodne prijave.");
        }

        [Test]
        public void Gradja_SvaTriPravilaSuKarikeIstogLanca()
        {
            Type baza = Odraz.Tip(Ns, "ValidationHandler");
            Assert.That(baza.IsAbstract, Is.True, "ValidationHandler mora biti apstraktan.");

            IEnumerable<string> nasljednici = Odraz.Nasljednici(baza).Select(t => t.Name);
            Assert.That(nasljednici, Is.SupersetOf(
                new[] { "UsernameValidator", "PasswordValidator", "AgeValidator" }));
        }

        [Test]
        public void Gradja_PraviloDrziReferencuNaSljedecePravilo()
        {
            Type baza = Odraz.Tip(Ns, "ValidationHandler");

            Assert.That(Odraz.DrziClanTipa(baza, baza), Is.True,
                "ValidationHandler mora drzati sljedece pravilo, i to kao ValidationHandler.");
        }

        [Test]
        public void Gradja_PraviloNeZnaZaKonkretnoSljedecePravilo()
        {
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "UsernameValidator"), Odraz.Tip(Ns, "PasswordValidator")),
                Is.False);
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "PasswordValidator"), Odraz.Tip(Ns, "AgeValidator")),
                Is.False);
        }
    }
}
