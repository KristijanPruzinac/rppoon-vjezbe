using System;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("C")]
    public class C2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Kompozit.C2";

        private static object Dio(string naziv, decimal cijena)
            => Odraz.Novi(Odraz.Tip(Ns, "BasicPart"), naziv, cijena);

        private static object Sklop(string naziv, int vlastiteMinute)
            => Odraz.Novi(Odraz.Tip(Ns, "Assembly"), naziv, vlastiteMinute);

        [Test]
        public void BasicPart_CijenaJeVlastitaAVrijemeNula()
        {
            object vijak = Dio("vijak", 0.50m);

            Assert.That(Odraz.Pozovi(vijak, "Cost"), Is.EqualTo(0.50m));
            Assert.That(Odraz.Pozovi(vijak, "AssemblyMinutes"), Is.EqualTo(0),
                "Osnovni dio se kupuje gotov, ne sastavlja se.");
        }

        [Test]
        public void Assembly_ZbrajaCijenePomnozeneKolicinom()
        {
            object sklop = Sklop("nosac", 5);
            Odraz.Pozovi(sklop, "Add", Dio("vijak", 0.50m), 4);
            Odraz.Pozovi(sklop, "Add", Dio("ploca", 10.00m), 1);

            Assert.That(Odraz.Pozovi(sklop, "Cost"), Is.EqualTo(12.00m));
        }

        [Test]
        public void Assembly_VrijemeJeVlastitoPlusPodsklopovi()
        {
            object sklop = Sklop("nosac", 5);
            Odraz.Pozovi(sklop, "Add", Dio("vijak", 0.50m), 4);

            Assert.That(Odraz.Pozovi(sklop, "AssemblyMinutes"), Is.EqualTo(5),
                "Osnovni dijelovi ne dodaju vrijeme, ostaje samo vlastito.");
        }

        [Test]
        public void KolicinaSeMnoziKrozStablo()
        {
            // Ovo je zamka zadatka. Podsklop ima 3 vijka; glavni sklop
            // sadrzi 2 takva podsklopa -> 6 vijaka, ne 3.
            object podsklop = Sklop("podsklop", 2);
            Odraz.Pozovi(podsklop, "Add", Dio("vijak", 1.00m), 3);

            object glavni = Sklop("glavni", 10);
            Odraz.Pozovi(glavni, "Add", podsklop, 2);

            Assert.That(Odraz.Pozovi(glavni, "Cost"), Is.EqualTo(6.00m),
                "Dva podsklopa po tri vijka daju sest vijaka.");
            Assert.That(Odraz.Pozovi(glavni, "AssemblyMinutes"), Is.EqualTo(14),
                "10 vlastitih + 2 x 2 minute po podsklopu.");
        }

        [Test]
        public void TriRazineUgnijezdenja()
        {
            object najdublji = Sklop("razina3", 1);
            Odraz.Pozovi(najdublji, "Add", Dio("matica", 2.00m), 2);

            object srednji = Sklop("razina2", 1);
            Odraz.Pozovi(srednji, "Add", najdublji, 3);

            object vanjski = Sklop("razina1", 1);
            Odraz.Pozovi(vanjski, "Add", srednji, 2);

            Assert.That(Odraz.Pozovi(vanjski, "Cost"), Is.EqualTo(24.00m),
                "2 x 3 x 2 matice po 2.00 = 24.00");
            Assert.That(Odraz.Pozovi(vanjski, "AssemblyMinutes"), Is.EqualTo(9),
                "1 + 2 x (1 + 3 x 1) = 9");
        }

        [Test]
        public void PrazanSklop_SamoVlastitoVrijemeINulaCijene()
        {
            object prazan = Sklop("prazan", 7);

            Assert.That(Odraz.Pozovi(prazan, "Cost"), Is.EqualTo(0m));
            Assert.That(Odraz.Pozovi(prazan, "AssemblyMinutes"), Is.EqualTo(7));
        }

        [Test]
        public void Remove_IzbacujeDioIzObracuna()
        {
            object ploca = Dio("ploca", 10.00m);
            object sklop = Sklop("nosac", 5);
            Odraz.Pozovi(sklop, "Add", Dio("vijak", 0.50m), 4);
            Odraz.Pozovi(sklop, "Add", ploca, 1);

            Odraz.Pozovi(sklop, "Remove", ploca);

            Assert.That(Odraz.Pozovi(sklop, "Cost"), Is.EqualTo(2.00m));
        }

        [Test]
        public void IstiDioDvaputSRazlicitomKolicinom()
        {
            object vijak = Dio("vijak", 1.00m);
            object sklop = Sklop("sklop", 0);
            Odraz.Pozovi(sklop, "Add", vijak, 2);
            Odraz.Pozovi(sklop, "Add", vijak, 3);

            Assert.That(Odraz.Pozovi(sklop, "Cost"), Is.EqualTo(5.00m),
                "Isti dio smije se pojaviti vise puta - kolicina pripada vezi, ne dijelu.");
        }

        [Test]
        public void KlijentNeRazlikujeOsnovniDioOdSklopa()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IPart");
            object osnovni = Dio("vijak", 1.00m);
            object sklop = Sklop("sklop", 3);

            Assert.That(sucelje.IsInstanceOfType(osnovni), Is.True);
            Assert.That(sucelje.IsInstanceOfType(sklop), Is.True);
            Assert.That(Odraz.Pozovi(osnovni, "Cost"), Is.EqualTo(1.00m));
            Assert.That(Odraz.Pozovi(sklop, "Cost"), Is.EqualTo(0m));
        }

        [Test]
        public void Gradja_ObaTipaDijeleKomponentu()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IPart");

            Assert.That(sucelje.IsAssignableFrom(Odraz.Tip(Ns, "BasicPart")), Is.True);
            Assert.That(sucelje.IsAssignableFrom(Odraz.Tip(Ns, "Assembly")), Is.True);
        }

        [Test]
        public void Gradja_OsnovniDioNemaDjecu()
        {
            Assert.That(
                Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "BasicPart"), Odraz.Sucelje(Ns, "IPart")),
                Is.False,
                "BasicPart je list i ne sadrzi druge dijelove.");
        }

        [Test]
        public void Gradja_SigurnaInacica_AddNijeNaSucelju()
        {
            Assert.That(Odraz.Sucelje(Ns, "IPart").GetMethod("Add"), Is.Null,
                "Add stoji samo na sklopu, ne na sucelju komponente.");
        }
    }
}
