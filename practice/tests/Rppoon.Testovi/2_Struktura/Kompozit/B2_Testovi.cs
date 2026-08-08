using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Kompozit.B2;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("B")]
    public class B2_Testovi
    {
        /// <summary>
        ///  Ana (direktorica, 5000)
        ///    Bruno (voditelj, 3000)
        ///      Ceco (2000)
        ///      Dino (2000)
        ///    Ema (2500)
        /// </summary>
        private static Manager Tvrtka()
        {
            Manager bruno = new Manager("Bruno", 3000m);
            bruno.Add(new Developer("Ceco", 2000m));
            bruno.Add(new Developer("Dino", 2000m));

            Manager ana = new Manager("Ana", 5000m);
            ana.Add(bruno);
            ana.Add(new Developer("Ema", 2500m));

            return ana;
        }

        [Test]
        public void Developer_PlacaJeVlastita()
        {
            Assert.That(new Developer("Ceco", 2000m).TotalSalary(), Is.EqualTo(2000m));
        }

        [Test]
        public void Manager_ZbrajaVlastituPlacuIPodredene()
        {
            Assert.That(Tvrtka().TotalSalary(), Is.EqualTo(14500m));
        }

        [Test]
        public void HeadCount_VoditeljBrojiISebe()
        {
            Assert.That(Tvrtka().HeadCount(), Is.EqualTo(5),
                "Ana, Bruno, Ceco, Dino i Ema - voditelji se broje kao ljudi.");
        }

        [Test]
        public void Depth_JeMaksimumPoDjeci_NeZbroj()
        {
            // Klasicna zamka: ako se dubina zbraja kao i ostalo, ispada 4.
            Assert.That(Tvrtka().Depth(), Is.EqualTo(3),
                "Ana -> Bruno -> Ceco je najdublji put, dakle 3.");
        }

        [Test]
        public void Depth_NesimetricnoStablo_UzimaNajdubljuGranu()
        {
            Manager duboko = new Manager("D1", 100m);
            Manager d2 = new Manager("D2", 100m);
            Manager d3 = new Manager("D3", 100m);
            d3.Add(new Developer("List", 100m));
            d2.Add(d3);
            duboko.Add(d2);
            duboko.Add(new Developer("Plitki", 100m));

            Assert.That(duboko.Depth(), Is.EqualTo(4));
        }

        [Test]
        public void VoditeljBezPodredenih_ImaDubinuJedan()
        {
            Manager sam = new Manager("Sam", 4000m);

            Assert.That(sam.Depth(), Is.EqualTo(1));
            Assert.That(sam.HeadCount(), Is.EqualTo(1));
            Assert.That(sam.TotalSalary(), Is.EqualTo(4000m));
        }

        [Test]
        public void Developer_ImaDubinuJedan()
        {
            Assert.That(new Developer("Ceco", 2000m).Depth(), Is.EqualTo(1));
        }

        [Test]
        public void Remove_UklanjaCijeloPodstablo()
        {
            Manager bruno = new Manager("Bruno", 3000m);
            bruno.Add(new Developer("Ceco", 2000m));

            Manager ana = new Manager("Ana", 5000m);
            ana.Add(bruno);
            ana.Add(new Developer("Ema", 2500m));

            ana.Remove(bruno);

            Assert.That(ana.HeadCount(), Is.EqualTo(2), "Ostaju Ana i Ema.");
            Assert.That(ana.TotalSalary(), Is.EqualTo(7500m), "Odlazi i Brunina i Cecina placa.");
            Assert.That(ana.Depth(), Is.EqualTo(2));
        }

        [Test]
        public void KlijentNeRazlikujeRazvijateljaOdVoditelja()
        {
            IEmployee list = new Developer("Ceco", 2000m);
            IEmployee kompozit = Tvrtka();

            Assert.That(list.HeadCount(), Is.EqualTo(1));
            Assert.That(kompozit.HeadCount(), Is.EqualTo(5));
        }

        [Test]
        public void Gradja_ListIKompozitDijeleKomponentu()
        {
            Assert.That(typeof(IEmployee).IsAssignableFrom(typeof(Developer)), Is.True);
            Assert.That(typeof(IEmployee).IsAssignableFrom(typeof(Manager)), Is.True);
        }

        [Test]
        public void Gradja_VoditeljDrziZbirkuKomponenti()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Manager), typeof(IEmployee)), Is.True);
        }

        [Test]
        public void Gradja_RazvijateljNemaPodredene()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Developer), typeof(IEmployee)), Is.False,
                "Developer je list i nema podredene.");
        }
    }
}
