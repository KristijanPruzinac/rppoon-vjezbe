using System.Linq;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Kompozit.A1;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("A")]
    public class A1_Testovi
    {
        // Product je u ovom zadatku DAN, pa ga se ne testira zasebno - takav
        // test prolazi i na praznom kosturu i nista ne govori o tvom radu.
        // Stopa poreza od 25 % svejedno se provjerava, kroz zbrojeve paketa.

        [Test]
        public void GiftSet_ZbrajaCijeneSadrzanihStavki()
        {
            GiftSet paket = new GiftSet("Poklon");
            paket.Add(new Product("T-Shirt", 100m));
            paket.Add(new Product("Salica", 20m));

            Assert.That(paket.CalculatePriceWithTax(), Is.EqualTo(150m));
        }

        [Test]
        public void PrazanGiftSet_StajeNula()
        {
            Assert.That(new GiftSet("Prazan").CalculatePriceWithTax(), Is.EqualTo(0m));
        }

        [Test]
        public void GiftSet_MozeSadrzavatiDrugiGiftSet()
        {
            // Rekurzija je razlog zasto obrazac postoji: paket u paketu
            // obraduje se istim pozivom, bez ijedne posebne grane.
            GiftSet unutarnji = new GiftSet("Mali paket");
            unutarnji.Add(new Product("Salica", 20m));
            unutarnji.Add(new Product("Privjesak", 8m));

            GiftSet vanjski = new GiftSet("Veliki paket");
            vanjski.Add(new Product("T-Shirt", 100m));
            vanjski.Add(unutarnji);

            Assert.That(vanjski.CalculatePriceWithTax(), Is.EqualTo(160m));
        }

        [Test]
        public void DubokoUgnijezdenjeSeCijeloObilazi()
        {
            GiftSet najdublji = new GiftSet("3");
            najdublji.Add(new Product("Bombon", 4m));

            GiftSet srednji = new GiftSet("2");
            srednji.Add(najdublji);

            GiftSet vanjski = new GiftSet("1");
            vanjski.Add(srednji);

            Assert.That(vanjski.CalculatePriceWithTax(), Is.EqualTo(5m));
        }

        [Test]
        public void Remove_IzbacujeStavkuIzZbroja()
        {
            GiftSet paket = new GiftSet("Poklon");
            Product salica = new Product("Salica", 20m);
            paket.Add(new Product("T-Shirt", 100m));
            paket.Add(salica);

            paket.Remove(salica);

            Assert.That(paket.Count, Is.EqualTo(1));
            Assert.That(paket.CalculatePriceWithTax(), Is.EqualTo(125m));
        }

        [Test]
        public void KlijentNeRazlikujeListOdKompozita()
        {
            // Isti poziv na oba tipa - to je obecanje obrasca.
            IBuyable list = new Product("T-Shirt", 100m);
            GiftSet slozeni = new GiftSet("Paket");
            slozeni.Add(new Product("T-Shirt", 100m));
            IBuyable kompozit = slozeni;

            Assert.That(list.CalculatePriceWithTax(), Is.EqualTo(kompozit.CalculatePriceWithTax()));
        }

        [Test]
        public void Gradja_ListIKompozitDijeleKomponentu()
        {
            Assert.That(typeof(IBuyable).IsAssignableFrom(typeof(Product)), Is.True);
            Assert.That(typeof(IBuyable).IsAssignableFrom(typeof(GiftSet)), Is.True);
        }

        [Test]
        public void Gradja_KompozitDrziZbirkuKomponenti()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(GiftSet), typeof(IBuyable)), Is.True,
                "GiftSet mora drzati zbirku tipa IBuyable, a ne zbirku konkretnih proizvoda.");
        }

        [Test]
        public void Gradja_SigurnaInacica_AddNijeNaKomponenti()
        {
            // Kolegij uci "sigurnu" inacicu: Add/Remove stoje na kompozitu.
            // Da su na IBuyable, Product bi morao imati besmislen Add.
            Assert.That(typeof(IBuyable).GetMethods().Any(m => m.Name == "Add"), Is.False,
                "U sigurnoj inacici sucelje komponente nema Add.");
            Assert.That(typeof(GiftSet).GetMethod("Add"), Is.Not.Null,
                "Add mora postojati na kompozitu.");
        }
    }
}
