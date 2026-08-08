using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Promatrac.A2;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("A")]
    public class A2_Testovi
    {
        [Test]
        public void CropBuyer_KupujeKadCijenaPadneNaPrag()
        {
            CropStockMarket burza = new CropStockMarket();
            CropBuyer kupac = new CropBuyer(3.00m);
            burza.Add(kupac);

            burza.OnUpdate(5.00m);
            Assert.That(kupac.Purchases, Is.EqualTo(0), "Iznad praga se ne kupuje.");

            burza.OnUpdate(3.00m);
            Assert.That(kupac.Purchases, Is.EqualTo(1), "Granica je ukljuciva.");

            burza.OnUpdate(2.50m);
            Assert.That(kupac.Purchases, Is.EqualTo(2));
        }

        [Test]
        public void CropView_PrikazujeCijenuSDvijeDecimale()
        {
            CropStockMarket burza = new CropStockMarket();
            CropView prikaz = new CropView();
            burza.Add(prikaz);

            burza.OnUpdate(3.5m);

            Assert.That(prikaz.Display, Is.EqualTo(3.5m.ToString("F2") + " EUR"));
        }

        [Test]
        public void CropLogger_BiljeziSvakuPromjenu()
        {
            CropStockMarket burza = new CropStockMarket();
            CropLogger dnevnik = new CropLogger();
            burza.Add(dnevnik);

            burza.OnUpdate(1.0m);
            burza.OnUpdate(2.0m);

            Assert.That(dnevnik.History, Is.EqualTo(new[] { 1.0m, 2.0m }));
        }

        [Test]
        public void JedanDogadaj_TriRazliciteReakcije()
        {
            // Srz obrasca: subjekt salje jednu obavijest, a svaki promatrac
            // radi svoje - i nijedan ne zna da ostali postoje.
            CropStockMarket burza = new CropStockMarket();
            CropBuyer kupac = new CropBuyer(2.00m);
            CropView prikaz = new CropView();
            CropLogger dnevnik = new CropLogger();
            burza.Add(kupac);
            burza.Add(prikaz);
            burza.Add(dnevnik);

            burza.OnUpdate(1.75m);

            Assert.That(kupac.Purchases, Is.EqualTo(1));
            Assert.That(prikaz.Display, Is.EqualTo(1.75m.ToString("F2") + " EUR"));
            Assert.That(dnevnik.History, Is.EqualTo(new[] { 1.75m }));
        }

        [Test]
        public void Remove_OdjavljeniPromatracViseNeReagira()
        {
            CropStockMarket burza = new CropStockMarket();
            CropLogger dnevnik = new CropLogger();
            burza.Add(dnevnik);
            burza.OnUpdate(1.0m);

            burza.Remove(dnevnik);
            burza.OnUpdate(9.0m);

            Assert.That(dnevnik.History, Is.EqualTo(new[] { 1.0m }));
        }

        [Test]
        public void Gradja_SvaTriPratiteljaDijeleSucelje()
        {
            Assert.That(Odraz.Implementacije(typeof(ICropTracker)).Count, Is.GreaterThanOrEqualTo(3),
                "Kupac, prikaz i dnevnik moraju dijeliti sucelje ICropTracker.");
        }

        [Test]
        public void Gradja_BurzaDrziZbirkuPratitelja()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(CropStockMarket), typeof(ICropTracker)), Is.True);
        }
    }
}
