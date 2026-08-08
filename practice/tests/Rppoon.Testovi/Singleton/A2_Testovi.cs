using System;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Singleton.A2;

namespace Rppoon.Testovi.Singleton
{
    [TestFixture]
    [Category("Singleton")]
    [Category("A")]
    public class A2_Testovi
    {
        // Singleton zivi kroz cijeli test-proces, pa svaki test koristi
        // vlastiti kljuc - inace bi redoslijed izvodenja mijenjao rezultat.
        private static string Kljuc()
        {
            return "kljuc-" + Guid.NewGuid().ToString("N");
        }

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(AppConfig.Instance, Is.SameAs(AppConfig.Instance));
        }

        [Test]
        public void Set_ZatimGet_VracaZapisanuVrijednost()
        {
            string kljuc = Kljuc();

            AppConfig.Instance.Set(kljuc, "tamna");

            Assert.That(AppConfig.Instance.Get(kljuc), Is.EqualTo("tamna"));
        }

        [Test]
        public void Set_PostojeciKljuc_Prepisuje()
        {
            string kljuc = Kljuc();
            AppConfig.Instance.Set(kljuc, "prva");

            AppConfig.Instance.Set(kljuc, "druga");

            Assert.That(AppConfig.Instance.Get(kljuc), Is.EqualTo("druga"));
        }

        [Test]
        public void Get_NepoznatKljuc_VracaNull()
        {
            Assert.That(AppConfig.Instance.Get(Kljuc()), Is.Null);
        }

        [Test]
        public void Postavka_ZapisanaJednom_VidljivaJeSvimaKojiTrazeInstance()
        {
            // Ovo je razlog zasto je Singleton i koristan i opasan:
            // stanje je zajednicko cijeloj aplikaciji.
            string kljuc = Kljuc();

            AppConfig prvi = AppConfig.Instance;
            prvi.Set(kljuc, "vrijednost");

            AppConfig drugi = AppConfig.Instance;

            Assert.That(drugi.Get(kljuc), Is.EqualTo("vrijednost"));
        }

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            Assert.That(Odraz.NemaJavnogKonstruktora(typeof(AppConfig)), Is.True,
                "AppConfig ne smije imati javni konstruktor.");
        }

        [Test]
        public void Gradja_InstanceJeStaticnaTockaPristupa()
        {
            PropertyInfo svojstvo = typeof(AppConfig).GetProperty(
                "Instance", BindingFlags.Static | BindingFlags.Public);

            Assert.That(svojstvo, Is.Not.Null, "Instance mora biti javno i staticno.");
            Assert.That(svojstvo.PropertyType, Is.EqualTo(typeof(AppConfig)));
        }
    }
}
