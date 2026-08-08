using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Strategija.B1;

namespace Rppoon.Testovi.Strategija
{
    [TestFixture]
    [Category("Strategija")]
    [Category("B")]
    public class B1_Testovi
    {
        [Test]
        public void ZipCompression_MijenjaNastavakUZip()
        {
            Assert.That(new ZipCompression().Compress("izvjestaj.txt"), Is.EqualTo("izvjestaj.zip"));
        }

        [Test]
        public void RarCompression_MijenjaNastavakURar()
        {
            Assert.That(new RarCompression().Compress("izvjestaj.txt"), Is.EqualTo("izvjestaj.rar"));
        }

        [Test]
        public void NazivBezTocke_DobivaNastavakNaKraj()
        {
            Assert.That(new ZipCompression().Compress("podaci"), Is.EqualTo("podaci.zip"));
        }

        [Test]
        public void VisestrukeTocke_MijenjaSeSamoZadnjiNastavak()
        {
            Assert.That(new ZipCompression().Compress("arhiva.2024.txt"), Is.EqualTo("arhiva.2024.zip"));
        }

        [Test]
        public void Archiver_DelegiraPoslaStrategiji()
        {
            Archiver arhiver = new Archiver(new ZipCompression());

            Assert.That(arhiver.Archive("biljeske.md"), Is.EqualTo("biljeske.zip"));
        }

        [Test]
        public void ZamjenaStrategijeUHodu_MijenjaNastavak()
        {
            Archiver arhiver = new Archiver(new ZipCompression());
            Assert.That(arhiver.Archive("biljeske.md"), Is.EqualTo("biljeske.zip"));

            arhiver.Strategy = new RarCompression();

            Assert.That(arhiver.Archive("biljeske.md"), Is.EqualTo("biljeske.rar"),
                "Isti arhiver, druga strategija, drugi rezultat - bez ijednog if-a.");
        }

        [Test]
        public void Gradja_ArchiverDrziReferencuNaSucelje()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(Archiver), typeof(ICompressionStrategy)), Is.True,
                "Archiver mora drzati clan tipa ICompressionStrategy.");
        }

        [Test]
        public void Gradja_ObjeStrategijeImplementirajuSucelje()
        {
            Assert.That(typeof(ICompressionStrategy).IsAssignableFrom(typeof(ZipCompression)), Is.True);
            Assert.That(typeof(ICompressionStrategy).IsAssignableFrom(typeof(RarCompression)), Is.True);
        }
    }
}
