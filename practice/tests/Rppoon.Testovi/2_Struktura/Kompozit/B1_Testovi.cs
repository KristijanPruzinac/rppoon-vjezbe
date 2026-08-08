using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Kompozit.B1;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("B")]
    public class B1_Testovi
    {
        /// <summary>
        ///  korijen/
        ///    biljeske.txt      (5)
        ///    slike/
        ///      more.jpg        (300)
        ///      planina.jpg     (200)
        ///      arhiva/
        ///        staro.png     (50)
        /// </summary>
        private static Folder Stablo()
        {
            Folder arhiva = new Folder("arhiva");
            arhiva.Add(new File("staro.png", 50));

            Folder slike = new Folder("slike");
            slike.Add(new File("more.jpg", 300));
            slike.Add(new File("planina.jpg", 200));
            slike.Add(arhiva);

            Folder korijen = new Folder("korijen");
            korijen.Add(new File("biljeske.txt", 5));
            korijen.Add(slike);

            return korijen;
        }

        [Test]
        public void File_VracaVlastituVelicinu()
        {
            Assert.That(new File("a.txt", 42).Size(), Is.EqualTo(42));
        }

        [Test]
        public void Folder_ZbrajaVelicineKrozCijeloStablo()
        {
            Assert.That(Stablo().Size(), Is.EqualTo(555));
        }

        [Test]
        public void FileCount_BrojiSamoDatoteke()
        {
            Assert.That(Stablo().FileCount(), Is.EqualTo(4),
                "Mape se ne broje - u stablu su cetiri datoteke.");
        }

        [Test]
        public void PraznaMapa_NulaBajtovaINulaDatoteka()
        {
            Folder prazna = new Folder("prazna");

            Assert.That(prazna.Size(), Is.EqualTo(0));
            Assert.That(prazna.FileCount(), Is.EqualTo(0));
        }

        [Test]
        public void Find_PronalaziDatotekuDubokoUStablu()
        {
            IFileSystemItem nadeno = Stablo().Find("staro.png");

            Assert.That(nadeno, Is.Not.Null);
            Assert.That(nadeno.Name, Is.EqualTo("staro.png"));
            Assert.That(nadeno.Size(), Is.EqualTo(50));
        }

        [Test]
        public void Find_PronalaziIMapu()
        {
            IFileSystemItem nadeno = Stablo().Find("slike");

            Assert.That(nadeno, Is.Not.Null);
            Assert.That(nadeno.FileCount(), Is.EqualTo(3), "Mapa 'slike' sadrzi tri datoteke.");
        }

        [Test]
        public void Find_MapaProvjeravaPrvoSebe()
        {
            Assert.That(Stablo().Find("korijen").Name, Is.EqualTo("korijen"));
        }

        [Test]
        public void Find_NepostojeceIme_VracaNull()
        {
            Assert.That(Stablo().Find("nema.me"), Is.Null);
        }

        [Test]
        public void Remove_IzbacujePodstabloIzZbroja()
        {
            Folder korijen = Stablo();
            IFileSystemItem slike = korijen.Find("slike");

            korijen.Remove(slike);

            Assert.That(korijen.Size(), Is.EqualTo(5));
            Assert.That(korijen.FileCount(), Is.EqualTo(1));
        }

        [Test]
        public void Gradja_ListIKompozitDijeleKomponentu()
        {
            Assert.That(typeof(IFileSystemItem).IsAssignableFrom(typeof(File)), Is.True);
            Assert.That(typeof(IFileSystemItem).IsAssignableFrom(typeof(Folder)), Is.True);
        }

        [Test]
        public void Gradja_MapaDrziZbirkuKomponenti()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Folder), typeof(IFileSystemItem)), Is.True,
                "Folder mora drzati zbirku IFileSystemItem, a ne zasebne popise datoteka i mapa.");
        }

        [Test]
        public void Gradja_MapaNemaOdvojenePopiseZaDatotekeIMape()
        {
            // Cesta pogreska: dva popisa, List<File> i List<Folder>. Tada
            // svaka operacija mora dvaput proci i obrazac gubi smisao.
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Folder), typeof(File)), Is.False,
                "Ne smije postojati zaseban popis samo datoteka.");
        }
    }
}
