using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Memento.B2;

namespace Rppoon.Testovi.Memento
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Memento")]
    [Category("B")]
    public class B2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Memento.B2";

        [Test]
        public void PocetnePostavke()
        {
            ImageEditor uredivac = new ImageEditor();

            Assert.That(uredivac.Brightness, Is.EqualTo(0));
            Assert.That(uredivac.Contrast, Is.EqualTo(0));
            Assert.That(uredivac.Filter, Is.EqualTo("bez"));
        }

        [Test]
        public void PostavkeSeMijenjaju()
        {
            ImageEditor uredivac = new ImageEditor();

            uredivac.Adjust(40, -10, "sepija");

            Assert.That(uredivac.Brightness, Is.EqualTo(40));
            Assert.That(uredivac.Contrast, Is.EqualTo(-10));
            Assert.That(uredivac.Filter, Is.EqualTo("sepija"));
        }

        [Test]
        public void VracanjeSnimke_SveTriVrijednostiSeVracaju()
        {
            ImageEditor uredivac = new ImageEditor();
            uredivac.Adjust(40, -10, "sepija");
            IEditorMemento snimka = uredivac.Save();

            uredivac.Adjust(0, 90, "crno-bijelo");
            uredivac.Restore(snimka);

            Assert.That(uredivac.Brightness, Is.EqualTo(40));
            Assert.That(uredivac.Contrast, Is.EqualTo(-10));
            Assert.That(uredivac.Filter, Is.EqualTo("sepija"));
        }

        [Test]
        public void SnimkaSeNeMijenjaZajednoSUredivacem()
        {
            ImageEditor uredivac = new ImageEditor();
            uredivac.Adjust(10, 10, "toplo");
            IEditorMemento snimka = uredivac.Save();

            uredivac.Adjust(99, 99, "hladno");
            uredivac.Restore(snimka);

            Assert.That(uredivac.Brightness, Is.EqualTo(10),
                "Snimka pamti vrijednosti iz trenutka snimanja, ne gleda u uredivac.");
        }

        [Test]
        public void SnimkaJednogUredivacaVrijediIUDrugom()
        {
            ImageEditor prvi = new ImageEditor();
            prvi.Adjust(25, 5, "vintage");

            ImageEditor drugi = new ImageEditor();
            drugi.Restore(prvi.Save());

            Assert.That(drugi.Filter, Is.EqualTo("vintage"));
            Assert.That(drugi.Brightness, Is.EqualTo(25));
            Assert.That(prvi.Brightness, Is.EqualTo(25),
                "Vracanje u jednom uredivacu ne dira drugi.");
        }

        [Test]
        public void PovijestVracaSnimkeObrnutimRedom()
        {
            ImageEditor uredivac = new ImageEditor();
            EditHistory povijest = new EditHistory();

            uredivac.Adjust(1, 1, "a");
            povijest.Push(uredivac.Save());
            uredivac.Adjust(2, 2, "b");
            povijest.Push(uredivac.Save());

            Assert.That(povijest.Count, Is.EqualTo(2));

            uredivac.Restore(povijest.Pop());
            Assert.That(uredivac.Filter, Is.EqualTo("b"));

            uredivac.Restore(povijest.Pop());
            Assert.That(uredivac.Filter, Is.EqualTo("a"));

            Assert.That(povijest.Count, Is.EqualTo(0));
            Assert.That(povijest.Pop(), Is.Null);
        }

        [Test]
        public void AlatPonistavaZadnjuIzmjenu()
        {
            PhotoEditorApp alat = new PhotoEditorApp();

            alat.Adjust(30, 0, "sepija");
            alat.Adjust(80, 20, "crno-bijelo");

            Assert.That(alat.Undo(), Is.True);

            Assert.That(alat.Brightness, Is.EqualTo(30));
            Assert.That(alat.Filter, Is.EqualTo("sepija"));
        }

        [Test]
        public void AlatPonistavaSveDoPocetnihPostavki()
        {
            PhotoEditorApp alat = new PhotoEditorApp();
            alat.Adjust(30, 0, "sepija");
            alat.Adjust(80, 20, "crno-bijelo");

            alat.Undo();
            alat.Undo();

            Assert.That(alat.Brightness, Is.EqualTo(0));
            Assert.That(alat.Filter, Is.EqualTo("bez"));
            Assert.That(alat.Undo(), Is.False);
        }

        [Test]
        public void AlatBezIzmjena_UndoNeRadiNista()
        {
            PhotoEditorApp alat = new PhotoEditorApp();

            Assert.That(alat.Undo(), Is.False);
            Assert.That(alat.Filter, Is.EqualTo("bez"));
        }

        [Test]
        public void NakonPonistavanjaSeMozeNastaviti()
        {
            PhotoEditorApp alat = new PhotoEditorApp();
            alat.Adjust(30, 0, "sepija");
            alat.Adjust(80, 20, "crno-bijelo");

            alat.Undo();
            alat.Adjust(50, 50, "toplo");

            Assert.That(alat.Filter, Is.EqualTo("toplo"));
            alat.Undo();
            Assert.That(alat.Filter, Is.EqualTo("sepija"));
        }

        [Test]
        public void Gradja_SuceljeSnimkeJeNamjernoPrazno()
        {
            MemberInfo[] clanovi = typeof(IEditorMemento).GetMembers();

            Assert.That(clanovi, Is.Empty,
                "Cim sucelje snimke isto sto obeca, skrbnik to moze procitati - " +
                "a upravo to ovaj zadatak zabranjuje.");
        }

        [Test]
        public void Gradja_KonkretnaSnimkaNijeJavna()
        {
            string[] javniTipovi = Odraz.TipoviU(Ns)
                .Where(t => t.IsPublic || t.IsNestedPublic)
                .Select(t => t.Name)
                .OrderBy(n => n)
                .ToArray();

            Assert.That(javniTipovi, Is.EqualTo(new[]
            {
                "EditHistory", "IEditorMemento", "ImageEditor", "PhotoEditorApp"
            }), "Osim ova cetiri tipa, u ovom prostoru imena ne smije biti nijedan javan. " +
                "Razred snimke mora biti 'private' (ugnijezden u uredivac) ili 'internal'.");
        }

        [Test]
        public void Gradja_SkrbnikVidiSnimkuSamoKrozSucelje()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(EditHistory), typeof(ImageEditor)), Is.False,
                "Skrbnik ne poznaje tvorca.");

            Type[] konkretneSnimke = Odraz.TipoviU(Ns)
                .Where(t => t.IsClass && typeof(IEditorMemento).IsAssignableFrom(t))
                .ToArray();

            Assert.That(konkretneSnimke, Is.Not.Empty,
                "Netko mora implementirati IEditorMemento.");

            foreach (Type snimka in konkretneSnimke)
            {
                Assert.That(Odraz.DrziClanTipa(typeof(EditHistory), snimka), Is.False,
                    $"Skrbnik drzi clan tipa '{snimka.Name}'. Mora ga vidjeti samo kao IEditorMemento.");
            }
        }

        [Test]
        public void Gradja_TvoracNeVodiVlastituPovijest()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(ImageEditor), typeof(EditHistory)), Is.False);
            Assert.That(Odraz.DrziZbirkuTipa(typeof(ImageEditor), typeof(IEditorMemento)), Is.False,
                "Uredivac snima i vraca JEDNO stanje; cuvanje niza stanja je posao skrbnika.");
        }
    }
}
