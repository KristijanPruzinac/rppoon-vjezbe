using System;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Singleton
{
    [TestFixture]
    [Category("Stvaranje")]
    [Category("Singleton")]
    [Category("C")]
    public class C2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Singleton.C2";

        private static object Veza()
        {
            Type tip = Odraz.Tip(Ns, "DatabaseConnection");
            PropertyInfo svojstvo = tip.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);

            Assert.That(svojstvo, Is.Not.Null,
                "DatabaseConnection mora imati javno staticno svojstvo 'Instance'.");

            return svojstvo.GetValue(null);
        }

        // Singleton pamti stanje kroz cijeli test-proces. Zato svaki test
        // krece od zatvorene veze, a brojac otvaranja se usporeduje s
        // pocetnom vrijednoscu umjesto s apsolutnim brojem.
        [SetUp]
        public void PrijeSvakogTesta()
        {
            Odraz.Pozovi(Veza(), "Close");
        }

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(Veza(), Is.SameAs(Veza()));
        }

        [Test]
        public void Open_OtvaraVezu()
        {
            object veza = Veza();

            Odraz.Pozovi(veza, "Open");

            Assert.That(Odraz.Pozovi(veza, "IsOpen"), Is.True);
        }

        [Test]
        public void Close_ZatvaraVezu()
        {
            object veza = Veza();
            Odraz.Pozovi(veza, "Open");

            Odraz.Pozovi(veza, "Close");

            Assert.That(Odraz.Pozovi(veza, "IsOpen"), Is.False);
        }

        [Test]
        public void Open_PovecavaBrojOtvaranjaZaJedan()
        {
            object veza = Veza();
            int prije = (int)Odraz.Pozovi(veza, "OpenCount");

            Odraz.Pozovi(veza, "Open");

            Assert.That(Odraz.Pozovi(veza, "OpenCount"), Is.EqualTo(prije + 1));
        }

        [Test]
        public void Open_NaVecOtvorenojVezi_NeBrojiSeDvaput()
        {
            object veza = Veza();
            Odraz.Pozovi(veza, "Open");
            int prije = (int)Odraz.Pozovi(veza, "OpenCount");

            Odraz.Pozovi(veza, "Open");

            Assert.That(Odraz.Pozovi(veza, "OpenCount"), Is.EqualTo(prije),
                "Ponovljeni Open na otvorenoj vezi ne otvara novu vezu.");
            Assert.That(Odraz.Pozovi(veza, "IsOpen"), Is.True);
        }

        [Test]
        public void Close_NaVecZatvorenojVezi_NeRadiNista()
        {
            object veza = Veza();
            int prije = (int)Odraz.Pozovi(veza, "OpenCount");

            Odraz.Pozovi(veza, "Close");

            Assert.That(Odraz.Pozovi(veza, "IsOpen"), Is.False);
            Assert.That(Odraz.Pozovi(veza, "OpenCount"), Is.EqualTo(prije));
        }

        [Test]
        public void StanjeJeZajednicko_OtvaranjePrekoJedneReferenceVidiSeUDrugoj()
        {
            Odraz.Pozovi(Veza(), "Open");

            Assert.That(Odraz.Pozovi(Veza(), "IsOpen"), Is.True,
                "Singleton znaci jedna zajednicka veza, ne veza po referenci.");
        }

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            Assert.That(Odraz.NemaJavnogKonstruktora(Odraz.Tip(Ns, "DatabaseConnection")), Is.True,
                "'new DatabaseConnection()' izvan razreda ne smije biti moguc.");
        }
    }
}
