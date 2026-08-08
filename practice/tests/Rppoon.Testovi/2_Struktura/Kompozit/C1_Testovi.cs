using System;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("C")]
    public class C1_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Kompozit.C1";

        private static object Krug(double x, double y, double r)
            => Odraz.Novi(Odraz.Tip(Ns, "Circle"), x, y, r);

        private static object Pravokutnik(double x, double y, double s, double v)
            => Odraz.Novi(Odraz.Tip(Ns, "Rectangle"), x, y, s, v);

        private static object Slozeni() => Odraz.Novi(Odraz.Tip(Ns, "CompositeShape"));

        [Test]
        public void Circle_KrajnjeTockeIzSredistaIPolumjera()
        {
            object krug = Krug(10, 5, 3);

            Assert.That(Odraz.Pozovi(krug, "Draw"), Is.EqualTo("krug"));
            Assert.That(Odraz.Pozovi(krug, "Left"), Is.EqualTo(7.0));
            Assert.That(Odraz.Pozovi(krug, "Right"), Is.EqualTo(13.0));
            Assert.That(Odraz.Pozovi(krug, "Top"), Is.EqualTo(8.0));
        }

        [Test]
        public void Rectangle_KrajnjeTockeIzDonjegLijevogKuta()
        {
            object pravokutnik = Pravokutnik(2, 1, 4, 6);

            Assert.That(Odraz.Pozovi(pravokutnik, "Draw"), Is.EqualTo("pravokutnik"));
            Assert.That(Odraz.Pozovi(pravokutnik, "Left"), Is.EqualTo(2.0));
            Assert.That(Odraz.Pozovi(pravokutnik, "Right"), Is.EqualTo(6.0));
            Assert.That(Odraz.Pozovi(pravokutnik, "Top"), Is.EqualTo(7.0));
        }

        [Test]
        public void CompositeShape_ObuhvacaSveSadrzaneOblike()
        {
            object slozeni = Slozeni();
            Odraz.Pozovi(slozeni, "Add", Krug(10, 5, 3));        // 7..13, vrh 8
            Odraz.Pozovi(slozeni, "Add", Pravokutnik(2, 1, 4, 6)); // 2..6, vrh 7

            Assert.That(Odraz.Pozovi(slozeni, "Left"), Is.EqualTo(2.0), "Najlijeviji je pravokutnik.");
            Assert.That(Odraz.Pozovi(slozeni, "Right"), Is.EqualTo(13.0), "Najdesniji je krug.");
            Assert.That(Odraz.Pozovi(slozeni, "Top"), Is.EqualTo(8.0), "Najvisi je krug.");
        }

        [Test]
        public void CompositeShape_DrawSpajaOpiseDjece()
        {
            object slozeni = Slozeni();
            Odraz.Pozovi(slozeni, "Add", Krug(0, 0, 1));
            Odraz.Pozovi(slozeni, "Add", Pravokutnik(0, 0, 1, 1));

            Assert.That(Odraz.Pozovi(slozeni, "Draw"), Is.EqualTo("krug+pravokutnik"));
        }

        [Test]
        public void SlozeniUSlozenom_ObuhvacaSve()
        {
            // Zadatak izricito trazi slozene oblike od drugih slozenih oblika.
            object unutarnji = Slozeni();
            Odraz.Pozovi(unutarnji, "Add", Krug(100, 0, 5));       // 95..105

            object vanjski = Slozeni();
            Odraz.Pozovi(vanjski, "Add", Pravokutnik(0, 0, 2, 2)); // 0..2
            Odraz.Pozovi(vanjski, "Add", unutarnji);

            Assert.That(Odraz.Pozovi(vanjski, "Left"), Is.EqualTo(0.0));
            Assert.That(Odraz.Pozovi(vanjski, "Right"), Is.EqualTo(105.0));
            Assert.That(Odraz.Pozovi(vanjski, "Draw"), Is.EqualTo("pravokutnik+krug"));
        }

        [Test]
        public void NegativneKoordinate_RadeIspravno()
        {
            object slozeni = Slozeni();
            Odraz.Pozovi(slozeni, "Add", Krug(-10, 0, 2));   // -12..-8
            Odraz.Pozovi(slozeni, "Add", Krug(5, 0, 1));     // 4..6

            Assert.That(Odraz.Pozovi(slozeni, "Left"), Is.EqualTo(-12.0),
                "Uz negativne koordinate pocetna vrijednost 0 dala bi krivi rezultat.");
            Assert.That(Odraz.Pozovi(slozeni, "Right"), Is.EqualTo(6.0));
        }

        [Test]
        public void PrazanSlozeniOblik_VracaNuleIPrazanNiz()
        {
            object prazan = Slozeni();

            Assert.That(Odraz.Pozovi(prazan, "Draw"), Is.EqualTo(string.Empty));
            Assert.That(Odraz.Pozovi(prazan, "Left"), Is.EqualTo(0.0));
            Assert.That(Odraz.Pozovi(prazan, "Right"), Is.EqualTo(0.0));
            Assert.That(Odraz.Pozovi(prazan, "Top"), Is.EqualTo(0.0));
        }

        [Test]
        public void Remove_IzbacujeOblikIzObuhvata()
        {
            object slozeni = Slozeni();
            object krug = Krug(100, 0, 5);
            Odraz.Pozovi(slozeni, "Add", Pravokutnik(0, 0, 2, 2));
            Odraz.Pozovi(slozeni, "Add", krug);

            Odraz.Pozovi(slozeni, "Remove", krug);

            Assert.That(Odraz.Pozovi(slozeni, "Right"), Is.EqualTo(2.0));
            Assert.That(Odraz.Pozovi(slozeni, "Draw"), Is.EqualTo("pravokutnik"));
        }

        [Test]
        public void Gradja_SviObliciDijeleKomponentu()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IShape");

            foreach (string naziv in new[] { "Circle", "Rectangle", "CompositeShape" })
            {
                Assert.That(sucelje.IsAssignableFrom(Odraz.Tip(Ns, naziv)), Is.True,
                    $"'{naziv}' mora implementirati IShape.");
            }
        }

        [Test]
        public void Gradja_SlozeniOblikDrziZbirkuKomponenti()
        {
            Assert.That(
                Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "CompositeShape"), Odraz.Sucelje(Ns, "IShape")),
                Is.True,
                "CompositeShape mora drzati zbirku tipa IShape.");
        }

        [Test]
        public void Gradja_OsnovniObliciNemajuDjecu()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IShape");

            Assert.That(Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "Circle"), sucelje), Is.False);
            Assert.That(Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "Rectangle"), sucelje), Is.False);
        }

        [Test]
        public void Gradja_SigurnaInacica_AddNijeNaSucelju()
        {
            Assert.That(Odraz.Sucelje(Ns, "IShape").GetMethod("Add"), Is.Null,
                "U sigurnoj inacici Add stoji samo na slozenom obliku.");
        }
    }
}
