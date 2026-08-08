using System.Collections.Generic;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Strategija.A1;

namespace Rppoon.Testovi.Strategija
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Strategija")]
    [Category("A")]
    public class A1_Testovi
    {
        private static List<string> Imena()
        {
            return new List<string> { "Marija", "Ana", "Lucija", "Dijana", "Bero" };
        }

        [Test]
        public void AscendingSort_SortiraRastuce()
        {
            SortedList lista = new SortedList(Imena(), new AscendingSort());

            lista.Sort();

            Assert.That(lista.Names, Is.EqualTo(new[] { "Ana", "Bero", "Dijana", "Lucija", "Marija" }));
        }

        [Test]
        public void DescendingSort_SortiraPadajuce()
        {
            SortedList lista = new SortedList(Imena(), new DescendingSort());

            lista.Sort();

            Assert.That(lista.Names, Is.EqualTo(new[] { "Marija", "Lucija", "Dijana", "Bero", "Ana" }));
        }

        [Test]
        public void ZamjenaStrategijeUHodu_MijenjaPoredak()
        {
            // Ovo je SRZ obrasca: isti objekt, drugo ponasanje, bez ijednog if-a.
            SortedList lista = new SortedList(Imena(), new AscendingSort());
            lista.Sort();
            string prvoRastuce = lista.Names[0];

            lista.Strategy = new DescendingSort();
            lista.Sort();
            string prvoPadajuce = lista.Names[0];

            Assert.That(prvoRastuce, Is.EqualTo("Ana"));
            Assert.That(prvoPadajuce, Is.EqualTo("Marija"));
        }

        [Test]
        public void Add_DodanoImeSudjelujeUSortiranju()
        {
            SortedList lista = new SortedList(Imena(), new AscendingSort());

            lista.Add("Ada");
            lista.Sort();

            Assert.That(lista.Names[0], Is.EqualTo("Ada"));
        }

        [Test]
        public void Gradja_KontekstDrziReferencuNaApstraktnuStrategiju()
        {
            // Bez ove provjere zadatak bi se mogao "rijesiti" tako da
            // SortedList sam poziva list.Sort() - a to nije Strategija.
            Assert.That(
                Odraz.DrziClanTipa(typeof(SortedList), typeof(ISortStrategy)),
                Is.True,
                "Kontekst SortedList mora drzati clan tipa ISortStrategy (kompozicija).");
        }

        [Test]
        public void Gradja_ObjeStrategijeImplementirajuIstoSucelje()
        {
            Assert.That(typeof(ISortStrategy).IsAssignableFrom(typeof(AscendingSort)), Is.True);
            Assert.That(typeof(ISortStrategy).IsAssignableFrom(typeof(DescendingSort)), Is.True);
        }
    }
}
