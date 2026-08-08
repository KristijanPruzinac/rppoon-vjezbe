using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Singleton.B2;

namespace Rppoon.Testovi.Singleton
{
    [TestFixture]
    [Category("Stvaranje")]
    [Category("Singleton")]
    [Category("B")]
    public class B2_Testovi
    {
        private static string Korisnik()
        {
            return "korisnik-" + Guid.NewGuid().ToString("N");
        }

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(SessionCache.Instance, Is.SameAs(SessionCache.Instance));
        }

        [Test]
        public void Instance_IzDvjestoIstovremenihPoziva_PostojiTocnoJedanPrimjerak()
        {
            // Ovo je cijela poanta zadatka. Naivna lijena inacica ovdje pada.
            SessionCache[] dobiveni = new SessionCache[200];

            Parallel.For(0, dobiveni.Length, i => dobiveni[i] = SessionCache.Instance);

            Assert.That(dobiveni.Distinct().Count(), Is.EqualTo(1),
                "Pod istovremenim pozivima nastao je vise od jednog primjerka.");
        }

        [Test]
        public void Put_ZatimGet_VracaSpremljenuVrijednost()
        {
            string korisnik = Korisnik();

            SessionCache.Instance.Put(korisnik, "zeton");

            Assert.That(SessionCache.Instance.Get(korisnik), Is.EqualTo("zeton"));
        }

        [Test]
        public void Get_NepoznatKorisnik_VracaNull()
        {
            Assert.That(SessionCache.Instance.Get(Korisnik()), Is.Null);
        }

        [Test]
        public void Put_IzViseDretvi_NistaSeNeIzgubi()
        {
            string[] korisnici = Enumerable.Range(0, 100).Select(i => Korisnik()).ToArray();

            Parallel.ForEach(korisnici, k => SessionCache.Instance.Put(k, "v-" + k));

            foreach (string k in korisnici)
            {
                Assert.That(SessionCache.Instance.Get(k), Is.EqualTo("v-" + k),
                    "Istovremeni upisi ne smiju pregaziti jedan drugoga.");
            }
        }

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            Assert.That(Odraz.NemaJavnogKonstruktora(typeof(SessionCache)), Is.True);
        }
    }
}
