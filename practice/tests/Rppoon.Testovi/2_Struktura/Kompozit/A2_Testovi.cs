using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Kompozit.A2;

namespace Rppoon.Testovi.Kompozit
{
    [TestFixture]
    [Category("Struktura")]
    [Category("Kompozit")]
    [Category("A")]
    public class A2_Testovi
    {
        [Test]
        public void Task_ProcjenaJeVlastitiBrojSati()
        {
            Project projekt = new Project("Diplomski");
            projekt.Add(new Task("Uvod", 4, false));

            Assert.That(projekt.EstimateHours(), Is.EqualTo(4));
        }

        [Test]
        public void Task_DovrsenSeBrojiKaoJedan()
        {
            Project projekt = new Project("Diplomski");
            projekt.Add(new Task("Uvod", 4, true));
            projekt.Add(new Task("Razrada", 10, false));

            Assert.That(projekt.CompletedCount(), Is.EqualTo(1));
        }

        [Test]
        public void Projekt_ZbrajaProcjeneSvojihStavki()
        {
            Project projekt = new Project("Diplomski");
            projekt.Add(new Task("Uvod", 4, false));
            projekt.Add(new Task("Razrada", 10, false));
            projekt.Add(new Task("Zakljucak", 2, false));

            Assert.That(projekt.EstimateHours(), Is.EqualTo(16));
        }

        [Test]
        public void PodprojektSeUracunavaUNadredeni()
        {
            Project poglavlje = new Project("Poglavlje 2");
            poglavlje.Add(new Task("Teorija", 6, true));
            poglavlje.Add(new Task("Primjeri", 8, false));

            Project diplomski = new Project("Diplomski");
            diplomski.Add(new Task("Uvod", 4, true));
            diplomski.Add(poglavlje);

            Assert.That(diplomski.EstimateHours(), Is.EqualTo(18));
            Assert.That(diplomski.CompletedCount(), Is.EqualTo(2),
                "Dovrsene stavke broje se kroz cijelo stablo, ne samo na prvoj razini.");
        }

        [Test]
        public void PrazanProjekt_NulaSati_PaRasteDodavanjem()
        {
            // Prazan projekt sam po sebi ne dokazuje nista - kompozit je u
            // ovom zadatku DAN. Zato test odmah nastavlja na dodanu stavku.
            Project projekt = new Project("Prazan");
            Assert.That(projekt.EstimateHours(), Is.EqualTo(0));
            Assert.That(projekt.CompletedCount(), Is.EqualTo(0));

            projekt.Add(new Task("Prva stavka", 3, true));

            Assert.That(projekt.EstimateHours(), Is.EqualTo(3));
            Assert.That(projekt.CompletedCount(), Is.EqualTo(1));
        }

        [Test]
        public void Remove_SmanjujeProcjenu()
        {
            Project projekt = new Project("Diplomski");
            Task razrada = new Task("Razrada", 10, false);
            projekt.Add(new Task("Uvod", 4, false));
            projekt.Add(razrada);

            projekt.Remove(razrada);

            Assert.That(projekt.EstimateHours(), Is.EqualTo(4));
        }

        [Test]
        public void ListIKompozitOdgovarajuNaIsteMetode()
        {
            IToDoItem list = new Task("Uvod", 4, true);
            Project p = new Project("Projekt");
            p.Add(new Task("Uvod", 4, true));
            IToDoItem kompozit = p;

            Assert.That(list.EstimateHours(), Is.EqualTo(kompozit.EstimateHours()));
            Assert.That(list.CompletedCount(), Is.EqualTo(kompozit.CompletedCount()));
        }

        [Test]
        public void Gradja_ListIKompozitDijeleKomponentu()
        {
            Assert.That(typeof(IToDoItem).IsAssignableFrom(typeof(Task)), Is.True);
            Assert.That(typeof(IToDoItem).IsAssignableFrom(typeof(Project)), Is.True);
        }

        [Test]
        public void Gradja_KompozitDrziZbirkuKomponenti()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Project), typeof(IToDoItem)), Is.True);
        }

        [Test]
        public void Gradja_ListNeDrziDjecu()
        {
            // List je kraj stabla. Ako Task drzi zbirku IToDoItem,
            // razlika izmedu lista i kompozita je nestala.
            Assert.That(Odraz.DrziZbirkuTipa(typeof(Task), typeof(IToDoItem)), Is.False,
                "Task je list i ne smije sadrzavati druge stavke.");
        }
    }
}
