using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Singleton.B1;

namespace Rppoon.Testovi.Singleton
{
    [TestFixture]
    [Category("Stvaranje")]
    [Category("Singleton")]
    [Category("B")]
    public class B1_Testovi
    {
        // Brojac se ne moze vratiti na nulu izmedu testova, pa nijedan test
        // ne smije tvrditi "Next() vraca 1". Tvrdimo samo odnose.

        [Test]
        public void Instance_UvijekVracaIstiPrimjerak()
        {
            Assert.That(IdGenerator.Instance, Is.SameAs(IdGenerator.Instance));
        }

        [Test]
        public void Next_SvakiPutVracaVeciBroj()
        {
            int prvi = IdGenerator.Instance.Next();
            int drugi = IdGenerator.Instance.Next();

            Assert.That(drugi, Is.GreaterThan(prvi));
        }

        [Test]
        public void Next_NikadNeIzdajeIstiBrojDvaput()
        {
            HashSet<int> izdani = new HashSet<int>();

            for (int i = 0; i < 100; i++)
            {
                Assert.That(izdani.Add(IdGenerator.Instance.Next()), Is.True,
                    "Isti identifikator izdan je dvaput.");
            }
        }

        [Test]
        public void Current_VracaZadnjiIzdaniBrojBezIzdavanjaNovog()
        {
            int izdani = IdGenerator.Instance.Next();

            Assert.That(IdGenerator.Instance.Current, Is.EqualTo(izdani));
            Assert.That(IdGenerator.Instance.Current, Is.EqualTo(izdani),
                "Citanje Current ne smije trositi brojeve.");
        }

        [Test]
        public void BrojacJeZajednicki_DvijeReferenceNastavljajuIstiNiz()
        {
            IdGenerator prvi = IdGenerator.Instance;
            IdGenerator drugi = IdGenerator.Instance;

            int a = prvi.Next();
            int b = drugi.Next();

            Assert.That(b, Is.EqualTo(a + 1),
                "Druga referenca mora nastaviti isti niz, a ne poceti svoj.");
        }

        [Test]
        public void Gradja_KonstruktorNijeJavan()
        {
            Assert.That(Odraz.NemaJavnogKonstruktora(typeof(IdGenerator)), Is.True,
                "Bez privatnog konstruktora svatko moze napraviti svoj brojac.");
        }
    }
}
