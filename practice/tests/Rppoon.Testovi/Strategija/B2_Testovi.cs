using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Strategija.B2;

namespace Rppoon.Testovi.Strategija
{
    [TestFixture]
    [Category("Strategija")]
    [Category("B")]
    public class B2_Testovi
    {
        [Test]
        public void StandardShipping_DvaIPolPoKilogramu()
        {
            Assert.That(new StandardShipping().Cost(4m), Is.EqualTo(10.00m));
        }

        [Test]
        public void ExpressShipping_PetPoKilogramuPlusTriFiksno()
        {
            Assert.That(new ExpressShipping().Cost(4m), Is.EqualTo(23.00m));
        }

        [Test]
        public void PickupShipping_NistaNeNaplacuje()
        {
            Assert.That(new PickupShipping().Cost(40m), Is.EqualTo(0m),
                "Osobno preuzimanje je besplatno bez obzira na tezinu.");
        }

        [Test]
        public void Order_ZbrajaRobuIDostavu()
        {
            Order narudzba = new Order(100m, 4m, new StandardShipping());

            Assert.That(narudzba.Total(), Is.EqualTo(110.00m));
        }

        [Test]
        public void ZamjenaDostaveUHodu_MijenjaUkupno()
        {
            Order narudzba = new Order(100m, 4m, new StandardShipping());
            Assert.That(narudzba.Total(), Is.EqualTo(110.00m));

            narudzba.Shipping = new PickupShipping();

            Assert.That(narudzba.Total(), Is.EqualTo(100m));
        }

        [Test]
        public void Gradja_OrderDrziReferencuNaSucelje()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(Order), typeof(IShippingStrategy)), Is.True,
                "Order mora drzati clan tipa IShippingStrategy, a ne birati cjenik grananjem.");
        }

        [Test]
        public void Gradja_TriNacinaDostaveDijeleSucelje()
        {
            Assert.That(Odraz.Implementacije(typeof(IShippingStrategy)).Count, Is.EqualTo(3));
        }
    }
}
