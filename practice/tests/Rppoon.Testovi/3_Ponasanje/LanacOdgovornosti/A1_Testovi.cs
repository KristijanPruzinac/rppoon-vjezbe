using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.LanacOdgovornosti.A1;

namespace Rppoon.Testovi.LanacOdgovornosti
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("LanacOdgovornosti")]
    [Category("A")]
    public class A1_Testovi
    {
        /// <summary>Lazna sluzba: uvijek isti odgovor, ali broji koliko je puta pitana.</summary>
        private class LaznaPrijava : IAuthenticationService
        {
            private readonly bool odgovor;

            public LaznaPrijava(bool odgovor)
            {
                this.odgovor = odgovor;
            }

            public int BrojPoziva { get; private set; }

            public bool IsAuthenticated(string user)
            {
                this.BrojPoziva++;
                return this.odgovor;
            }
        }

        private class LaznaPrava : IUserRolesService
        {
            private readonly bool odgovor;

            public LaznaPrava(bool odgovor)
            {
                this.odgovor = odgovor;
            }

            public int BrojPoziva { get; private set; }

            public bool HasAccess(string user, string resource)
            {
                this.BrojPoziva++;
                return this.odgovor;
            }
        }

        [Test]
        public void SviFiltriPropustaju_ZahtjevJeValjan()
        {
            AuthenticationFilter prijava = new AuthenticationFilter(new LaznaPrijava(true));
            AccessFilter prava = new AccessFilter(new LaznaPrava(true));
            prijava.SetNext(prava);

            Assert.That(prijava.IsValid(new Request("ana", "/izvjestaji")), Is.True);
        }

        [Test]
        public void PrviFiltarOdbija_DrugiSeUopceNePita()
        {
            LaznaPrava prava = new LaznaPrava(true);
            AuthenticationFilter prijava = new AuthenticationFilter(new LaznaPrijava(false));
            prijava.SetNext(new AccessFilter(prava));

            bool valjan = prijava.IsValid(new Request("nepoznat", "/izvjestaji"));

            Assert.That(valjan, Is.False);
            Assert.That(prava.BrojPoziva, Is.EqualTo(0),
                "Odbijeni zahtjev ne smije putovati dalje - lanac se prekida na prvom 'ne'.");
        }

        [Test]
        public void ZadnjiFiltarOdbija_ZahtjevPada()
        {
            AuthenticationFilter prijava = new AuthenticationFilter(new LaznaPrijava(true));
            prijava.SetNext(new AccessFilter(new LaznaPrava(false)));

            Assert.That(prijava.IsValid(new Request("ana", "/tajno")), Is.False);
        }

        [Test]
        public void SamFiltarBezSljedecega_PropustaZahtjev()
        {
            AuthenticationFilter prijava = new AuthenticationFilter(new LaznaPrijava(true));

            Assert.That(prijava.IsValid(new Request("ana", "/pocetna")), Is.True,
                "Zadnja karika koja nema primjedbi znaci da primjedbi nema nitko.");
        }

        [Test]
        public void RedoslijedFiltaraSeMozeObrnuti()
        {
            // Nijedan filtar ne smije pretpostavljati svoje mjesto u lancu.
            LaznaPrijava prijavaSluzba = new LaznaPrijava(true);
            AccessFilter prava = new AccessFilter(new LaznaPrava(false));
            prava.SetNext(new AuthenticationFilter(prijavaSluzba));

            bool valjan = prava.IsValid(new Request("ana", "/tajno"));

            Assert.That(valjan, Is.False);
            Assert.That(prijavaSluzba.BrojPoziva, Is.EqualTo(0),
                "Sada je AccessFilter prvi, pa on prekida lanac.");
        }

        [Test]
        public void TriKarikeULancu()
        {
            AuthenticationFilter prva = new AuthenticationFilter(new LaznaPrijava(true));
            AccessFilter druga = new AccessFilter(new LaznaPrava(true));
            LaznaPrava trecaSluzba = new LaznaPrava(false);
            AccessFilter treca = new AccessFilter(trecaSluzba);

            prva.SetNext(druga);
            druga.SetNext(treca);

            Assert.That(prva.IsValid(new Request("ana", "/izvjestaji")), Is.False);
            Assert.That(trecaSluzba.BrojPoziva, Is.EqualTo(1),
                "Zahtjev je morao doci do zadnje karike.");
        }

        [Test]
        public void Gradja_ObaFiltraSuKarikeIstogLanca()
        {
            Assert.That(typeof(RequestFilter).IsAssignableFrom(typeof(AuthenticationFilter)), Is.True);
            Assert.That(typeof(RequestFilter).IsAssignableFrom(typeof(AccessFilter)), Is.True);
        }

        [Test]
        public void Gradja_FiltarNeZnaZaKonkretanSljedeciFiltar()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(AuthenticationFilter), typeof(AccessFilter)), Is.False,
                "Filtar vidi sljedecega samo kao RequestFilter. Polje konkretnog tipa " +
                "znaci da se lanac ne moze presloziti bez izmjene razreda.");
            Assert.That(Odraz.DrziClanTipa(typeof(AccessFilter), typeof(AuthenticationFilter)), Is.False);
        }
    }
}
