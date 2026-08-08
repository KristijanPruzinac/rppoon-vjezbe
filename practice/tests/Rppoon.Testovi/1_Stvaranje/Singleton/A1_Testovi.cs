using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Singleton.A1;

namespace Rppoon.Testovi.Singleton
{
    [TestFixture]
    [Category("Stvaranje")]
    [Category("Singleton")]
    [Category("A")]
    public class A1_Testovi
    {
        // Singleton po prirodi zivi kroz cijeli test-proces, pa se testovi
        // ne smiju oslanjati na broj zapisa. Svaki koristi vlastitu oznaku.
        private static string Oznaka()
        {
            return "poruka-" + Guid.NewGuid().ToString("N");
        }

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(Logger.Instance, Is.SameAs(Logger.Instance),
                "Dva poziva Instance moraju dati istu referencu, ne dva objekta.");
        }

        [Test]
        public void Instance_StanjeSeCuvaIzmeduPoziva()
        {
            string oznaka = Oznaka();

            Logger.Instance.Log(oznaka);

            Assert.That(Logger.Instance.Entries, Does.Contain(oznaka),
                "Zapis napravljen preko jednog poziva mora biti vidljiv i preko sljedeceg.");
        }

        // NAPOMENA: ovdje NEMA provjere iz vise dretvi, i to namjerno.
        // Lijeni Singleton iz ovog zadatka ("if (instance == null) ...") u
        // vise dretvi zna stvoriti vise primjeraka - provjereno, 8 od 50
        // poziva dobilo je razlicit objekt. To nije studentova greska nego
        // poznato ogranicenje ove inacice; rjesava se zakljucavanjem, sto je
        // tema zadatka B2. Test koji bi to trazio ovdje obarao bi tocno
        // napisan zadatak.

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            // Ovo je jedina provjera koju se ne moze zaobici: bez privatnog
            // konstruktora bilo tko moze napisati 'new Logger()' i Singleton pada.
            Assert.That(Odraz.NemaJavnogKonstruktora(typeof(Logger)), Is.True,
                "Logger ne smije imati javni konstruktor - inace nije Singleton.");
        }

        [Test]
        public void Gradja_PostojiStaticnoPoljeZaPrimjerak()
        {
            IEnumerable<FieldInfo> staticnaPolja = typeof(Logger)
                .GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(Logger));

            Assert.That(staticnaPolja.Any(), Is.True,
                "Primjerak se mora cuvati u staticnom polju tipa Logger.");
        }

        [Test]
        public void Gradja_InstanceJeStaticnaTockaPristupa()
        {
            PropertyInfo svojstvo = typeof(Logger).GetProperty(
                "Instance", BindingFlags.Static | BindingFlags.Public);

            Assert.That(svojstvo, Is.Not.Null, "Instance mora biti javno i staticno.");
            Assert.That(svojstvo.PropertyType, Is.EqualTo(typeof(Logger)));
        }
    }
}
