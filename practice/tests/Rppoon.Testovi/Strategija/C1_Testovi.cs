using System;
using System.Linq;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Strategija
{
    /// <summary>
    /// Razina C: test NE referencira tvoje tipove pri prevodenju, nego ih
    /// trazi odrazom. Zato mozes zadatak poceti od prazne datoteke - ali
    /// nazivi moraju biti tocno onakvi kakve zadatak trazi.
    /// </summary>
    [TestFixture]
    [Category("Strategija")]
    [Category("C")]
    public class C1_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Strategija.C1";

        private static object Invoice(decimal iznos, string nazivStrategije)
        {
            object strategija = Odraz.Novi(Odraz.Tip(Ns, nazivStrategije));
            return Odraz.Novi(Odraz.Tip(Ns, "Invoice"), iznos, strategija);
        }

        [Test]
        public void StandardTax_DodajeDvadesetPetPosto()
        {
            object racun = Invoice(100m, "StandardTax");

            Assert.That(Odraz.Pozovi(racun, "Total"), Is.EqualTo(125m));
        }

        [Test]
        public void ReducedTax_DodajeTrinaestPosto()
        {
            object racun = Invoice(200m, "ReducedTax");

            Assert.That(Odraz.Pozovi(racun, "Total"), Is.EqualTo(226m));
        }

        [Test]
        public void ZeroTax_NeMijenjaIznos()
        {
            object racun = Invoice(80m, "ZeroTax");

            Assert.That(Odraz.Pozovi(racun, "Total"), Is.EqualTo(80m));
        }

        [Test]
        public void ZamjenaStrategijeUHodu_MijenjaUkupanIznos()
        {
            object racun = Invoice(100m, "StandardTax");
            Assert.That(Odraz.Pozovi(racun, "Total"), Is.EqualTo(125m));

            Odraz.Postavi(racun, "Strategy", Odraz.Novi(Odraz.Tip(Ns, "ZeroTax")));

            Assert.That(Odraz.Pozovi(racun, "Total"), Is.EqualTo(100m),
                "Nakon zamjene strategije isti racun mora dati drugi rezultat.");
        }

        [Test]
        public void Gradja_SveTriStrategijeImplementirajuITaxStrategy()
        {
            Type sucelje = Odraz.Sucelje(Ns, "ITaxStrategy");

            foreach (string naziv in new[] { "StandardTax", "ReducedTax", "ZeroTax" })
            {
                Assert.That(sucelje.IsAssignableFrom(Odraz.Tip(Ns, naziv)), Is.True,
                    $"'{naziv}' mora implementirati ITaxStrategy.");
            }
        }

        [Test]
        public void Gradja_SuceljeImaTocnoJednuMetoduCalculate()
        {
            Type sucelje = Odraz.Sucelje(Ns, "ITaxStrategy");

            var metode = sucelje.GetMethods();

            Assert.That(metode.Length, Is.EqualTo(1),
                "Strategija ima usko sucelje - tocno jednu operaciju.");
            Assert.That(metode[0].Name, Is.EqualTo("Calculate"));
            Assert.That(metode[0].ReturnType, Is.EqualTo(typeof(decimal)));
            Assert.That(metode[0].GetParameters().Select(p => p.ParameterType),
                Is.EqualTo(new[] { typeof(decimal) }));
        }

        [Test]
        public void Gradja_InvoiceDrziReferencuNaSucelje()
        {
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "Invoice"), Odraz.Sucelje(Ns, "ITaxStrategy")),
                Is.True,
                "Kontekst Invoice mora drzati clan tipa ITaxStrategy, a ne birati stopu grananjem.");
        }
    }
}
