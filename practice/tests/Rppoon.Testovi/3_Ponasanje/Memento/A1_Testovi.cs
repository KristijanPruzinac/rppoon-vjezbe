using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Memento.A1;

namespace Rppoon.Testovi.Memento
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Memento")]
    [Category("A")]
    public class A1_Testovi
    {
        [Test]
        public void SnimkaPamtiTekstUTrenutkuSnimanja()
        {
            TextEditor uredivac = new TextEditor();
            uredivac.Add("prvi dio");

            TextSnapshot snimka = uredivac.Store();

            Assert.That(snimka.Text, Is.EqualTo("prvi dio"));
        }

        [Test]
        public void VracanjeStanja_TekstJeOpetKakavJeBio()
        {
            TextEditor uredivac = new TextEditor();
            uredivac.Add("sacuvano");
            TextSnapshot snimka = uredivac.Store();

            uredivac.Add(" pa jos ovo");
            uredivac.Restore(snimka);

            Assert.That(uredivac.GetText(), Is.EqualTo("sacuvano"));
        }

        [Test]
        public void SnimkaJeODVOJENAOdUredivaca()
        {
            TextEditor uredivac = new TextEditor();
            uredivac.Add("pocetak");
            TextSnapshot snimka = uredivac.Store();

            uredivac.Add(" nastavak");

            Assert.That(snimka.Text, Is.EqualTo("pocetak"),
                "Snimka mora zapamtiti vrijednost, a ne gledati u uredivac. " +
                "Ako se mijenja zajedno s njim, to nije snimka nego prozor.");
        }

        [Test]
        public void NakonVracanjaSeMozeNastavitiPisati()
        {
            TextEditor uredivac = new TextEditor();
            uredivac.Add("abc");
            TextSnapshot snimka = uredivac.Store();
            uredivac.Add("def");

            uredivac.Restore(snimka);
            uredivac.Add("XY");

            Assert.That(uredivac.GetText(), Is.EqualTo("abcXY"),
                "Restore mora zamijeniti STANJE uredivaca, ne samo zapamtiti niz sa strane.");
        }

        [Test]
        public void VracanjeNaStarijuSnimku_NeMoraIciRedom()
        {
            TextEditor uredivac = new TextEditor();
            uredivac.Add("a");
            TextSnapshot prva = uredivac.Store();
            uredivac.Add("b");
            TextSnapshot druga = uredivac.Store();
            uredivac.Add("c");

            uredivac.Restore(prva);
            Assert.That(uredivac.GetText(), Is.EqualTo("a"));

            uredivac.Restore(druga);
            Assert.That(uredivac.GetText(), Is.EqualTo("ab"),
                "Svaka snimka stoji sama za sebe - vracanje unatrag ne trosi ostale.");
        }

        [Test]
        public void PraznoStanje_SnimkaIVracanjeRade()
        {
            TextEditor uredivac = new TextEditor();
            TextSnapshot prazna = uredivac.Store();

            uredivac.Add("nesto");
            uredivac.Restore(prazna);

            Assert.That(uredivac.GetText(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void ProzorPonistavaZadnjuPromjenu()
        {
            TextWindow prozor = new TextWindow(new TextEditor());

            prozor.OnSavePressed();
            prozor.OnTextEntered("Dragi ");
            prozor.OnSavePressed();
            prozor.OnTextEntered("dnevnice");

            prozor.OnUndoPressed();

            Assert.That(prozor.Text, Is.EqualTo("Dragi "));
        }

        [Test]
        public void ProzorPonistavaViseKoraka_SveDoPrazneStranice()
        {
            TextWindow prozor = new TextWindow(new TextEditor());

            prozor.OnSavePressed();
            prozor.OnTextEntered("jedan");
            prozor.OnSavePressed();
            prozor.OnTextEntered(" dva");

            prozor.OnUndoPressed();
            Assert.That(prozor.Text, Is.EqualTo("jedan"));

            prozor.OnUndoPressed();
            Assert.That(prozor.Text, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DvaUredivacaSuNeovisna()
        {
            TextEditor prvi = new TextEditor();
            TextEditor drugi = new TextEditor();

            prvi.Add("prvi");
            drugi.Add("drugi");
            TextSnapshot snimkaPrvog = prvi.Store();

            drugi.Restore(snimkaPrvog);

            Assert.That(drugi.GetText(), Is.EqualTo("prvi"));
            Assert.That(prvi.GetText(), Is.EqualTo("prvi"),
                "Vracanje stanja u jednom uredivacu ne smije dirati drugi.");
        }

        [Test]
        public void Gradja_TvoracNeVodiVlastituPovijest()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(TextEditor), typeof(SnapshotStorage)), Is.False,
                "Povijest je posao skrbnika. Da je tvorac sam vodi, obrazac ne bi imao smisla - " +
                "razred bi radio i svoj posao i posao pamcenja.");
            Assert.That(Odraz.DrziZbirkuTipa(typeof(TextEditor), typeof(TextSnapshot)), Is.False,
                "Isto vrijedi i za zbirku snimki unutar tvorca.");
        }

        [Test]
        public void Gradja_SkrbnikNeZnaZaTvorca()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(SnapshotStorage), typeof(TextEditor)), Is.False,
                "Skrbnik cuva snimke i ne poznaje onoga tko ih je napravio.");
        }

        [Test]
        public void Gradja_SnimkaSeIzvanaNeMozeMijenjati()
        {
            PropertyInfo[] pisiva = typeof(TextSnapshot)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.SetMethod != null && p.SetMethod.IsPublic)
                .ToArray();

            FieldInfo[] javnaPolja = typeof(TextSnapshot)
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => !f.IsInitOnly)
                .ToArray();

            Assert.That(pisiva, Is.Empty,
                "Snimka se izvana smije samo citati. Da se moze mijenjati, povijest vise nije povijest.");
            Assert.That(javnaPolja, Is.Empty);
        }
    }
}
