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
    public class C1_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Singleton.C1";

        private static object Spooler()
        {
            Type tip = Odraz.Tip(Ns, "PrintSpooler");
            PropertyInfo svojstvo = tip.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);

            Assert.That(svojstvo, Is.Not.Null,
                "PrintSpooler mora imati javno staticno svojstvo 'Instance'.");

            return svojstvo.GetValue(null);
        }

        /// <summary>Isprazni red da testovi ne ovise o redoslijedu izvodenja.</summary>
        private static void Isprazni()
        {
            object spooler = Spooler();
            while ((int)Odraz.Pozovi(spooler, "Pending") > 0)
            {
                Odraz.Pozovi(spooler, "PrintNext");
            }
        }

        [SetUp]
        public void PrijeSvakogTesta()
        {
            Isprazni();
        }

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(Spooler(), Is.SameAs(Spooler()));
        }

        [Test]
        public void Send_PovecavaBrojDokumenataNaCekanju()
        {
            object spooler = Spooler();

            Odraz.Pozovi(spooler, "Send", "ugovor.pdf");
            Odraz.Pozovi(spooler, "Send", "racun.pdf");

            Assert.That(Odraz.Pozovi(spooler, "Pending"), Is.EqualTo(2));
        }

        [Test]
        public void PrintNext_VracaNajstarijiDokument()
        {
            object spooler = Spooler();
            Odraz.Pozovi(spooler, "Send", "prvi.pdf");
            Odraz.Pozovi(spooler, "Send", "drugi.pdf");

            Assert.That(Odraz.Pozovi(spooler, "PrintNext"), Is.EqualTo("prvi.pdf"),
                "Red radi po nacelu prvi unutra - prvi van.");
            Assert.That(Odraz.Pozovi(spooler, "PrintNext"), Is.EqualTo("drugi.pdf"));
        }

        [Test]
        public void PrintNext_SmanjujeBrojNaCekanju()
        {
            object spooler = Spooler();
            Odraz.Pozovi(spooler, "Send", "jedini.pdf");

            Odraz.Pozovi(spooler, "PrintNext");

            Assert.That(Odraz.Pozovi(spooler, "Pending"), Is.EqualTo(0));
        }

        [Test]
        public void PrintNext_PrazanRed_VracaNull()
        {
            Assert.That(Odraz.Pozovi(Spooler(), "PrintNext"), Is.Null);
        }

        [Test]
        public void RedJeZajednicki_DokumentPoslanPrekoJedneReferenceVidiSeUDrugoj()
        {
            object prvi = Spooler();
            Odraz.Pozovi(prvi, "Send", "zajednicki.pdf");

            object drugi = Spooler();

            Assert.That(Odraz.Pozovi(drugi, "Pending"), Is.EqualTo(1),
                "Singleton znaci jedan zajednicki red, ne red po referenci.");
        }

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            Assert.That(Odraz.NemaJavnogKonstruktora(Odraz.Tip(Ns, "PrintSpooler")), Is.True,
                "'new PrintSpooler()' izvan razreda ne smije biti moguc.");
        }
    }
}
