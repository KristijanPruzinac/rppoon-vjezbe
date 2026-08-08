using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Strategija.A2;

namespace Rppoon.Testovi.Strategija
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Strategija")]
    [Category("A")]
    public class A2_Testovi
    {
        private static Cart Kosarica(IDiscountStrategy popust)
        {
            Cart kosarica = new Cart(popust);
            kosarica.AddItem(30m);
            kosarica.AddItem(50m);
            kosarica.AddItem(20m);
            return kosarica;
        }

        [Test]
        public void NoDiscount_NeMijenjaIznos()
        {
            Assert.That(Kosarica(new NoDiscount()).Total(), Is.EqualTo(100m));
        }

        [Test]
        public void PercentageDiscount_UmanjujeZaPostotak()
        {
            Assert.That(Kosarica(new PercentageDiscount(20m)).Total(), Is.EqualTo(80m));
        }

        [Test]
        public void FixedDiscount_OduzimaFiksniIznos()
        {
            Assert.That(Kosarica(new FixedDiscount(15m)).Total(), Is.EqualTo(85m));
        }

        [Test]
        public void FixedDiscount_NikadNeIdeIspodNule()
        {
            Cart kosarica = new Cart(new FixedDiscount(500m));
            kosarica.AddItem(10m);

            Assert.That(kosarica.Total(), Is.EqualTo(0m),
                "Popust veci od iznosa ne smije dati negativan racun.");
        }

        [Test]
        public void ZamjenaPopustaUHodu_MijenjaUkupno()
        {
            Cart kosarica = Kosarica(new NoDiscount());
            Assert.That(kosarica.Total(), Is.EqualTo(100m));

            kosarica.Discount = new PercentageDiscount(50m);

            Assert.That(kosarica.Total(), Is.EqualTo(50m));
            Assert.That(kosarica.Subtotal(), Is.EqualTo(100m),
                "Popust mijenja samo ukupno za naplatu, ne i zbroj stavki.");
        }

        [Test]
        public void Gradja_KosaricaDrziReferencuNaSucelje()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(Cart), typeof(IDiscountStrategy)), Is.True,
                "Cart mora drzati clan tipa IDiscountStrategy, a ne racunati popust sam.");
        }

        [Test]
        public void Gradja_SveTriVrstePopustaDijeleSucelje()
        {
            Assert.That(Odraz.Implementacije(typeof(IDiscountStrategy)).Count, Is.GreaterThanOrEqualTo(3));
        }
    }
}
