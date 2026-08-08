using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Memento.A2;

namespace Rppoon.Testovi.Memento
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Memento")]
    [Category("A")]
    public class A2_Testovi
    {
        private static NavigationSession Sesija(string odrediste = "Osijek")
        {
            return new NavigationSession(new NavigationRoute(odrediste));
        }

        [Test]
        public void PraznaArhiva_NemaSnimki()
        {
            RouteArchive arhiva = new RouteArchive();

            Assert.That(arhiva.Count, Is.EqualTo(0));
            Assert.That(arhiva.Undo(), Is.Null,
                "Iz prazne arhive se vraca null, ne baca se iznimka.");
        }

        [Test]
        public void ArhivaVracaSnimkeObrnutimRedom()
        {
            NavigationRoute ruta = new NavigationRoute("Zagreb");
            RouteArchive arhiva = new RouteArchive();

            arhiva.Store(ruta.Save());
            ruta.AddWaypoint("Slavonski Brod");
            arhiva.Store(ruta.Save());

            Assert.That(arhiva.Count, Is.EqualTo(2));
            Assert.That(arhiva.Undo().Waypoints, Is.EqualTo(new[] { "Slavonski Brod" }),
                "Zadnja spremljena snimka izlazi prva.");
            Assert.That(arhiva.Undo().Waypoints, Is.Empty);
            Assert.That(arhiva.Count, Is.EqualTo(0),
                "Undo snimku SKIDA s hrpe, ne samo cita.");
        }

        [Test]
        public void SnimkaPreziviKasnijePromjeneRute()
        {
            NavigationRoute ruta = new NavigationRoute("Zagreb");
            ruta.AddWaypoint("Nasice");
            RouteArchive arhiva = new RouteArchive();

            arhiva.Store(ruta.Save());
            ruta.AddWaypoint("Pozega");
            ruta.Destination = "Rijeka";

            SavedRoute snimka = arhiva.Undo();

            Assert.That(snimka.Destination, Is.EqualTo("Zagreb"));
            Assert.That(snimka.Waypoints, Is.EqualTo(new[] { "Nasice" }));
        }

        [Test]
        public void PonistavanjeDodaneTocke()
        {
            NavigationSession sesija = Sesija();

            sesija.AddWaypoint("Vinkovci");
            sesija.AddWaypoint("Vukovar");

            Assert.That(sesija.UndoLastChange(), Is.True);
            Assert.That(sesija.Waypoints, Is.EqualTo(new[] { "Vinkovci" }));
        }

        [Test]
        public void PonistavanjePromjeneOdredista()
        {
            NavigationSession sesija = Sesija("Osijek");

            sesija.ChangeDestination("Split");
            Assert.That(sesija.Destination, Is.EqualTo("Split"));

            sesija.UndoLastChange();

            Assert.That(sesija.Destination, Is.EqualTo("Osijek"));
        }

        [Test]
        public void PonistavanjeIdeKorakPoKorakUnatrag()
        {
            NavigationSession sesija = Sesija("Osijek");

            sesija.AddWaypoint("Dakovo");
            sesija.ChangeDestination("Zadar");
            sesija.AddWaypoint("Gospic");

            sesija.UndoLastChange();
            Assert.That(sesija.Waypoints, Is.EqualTo(new[] { "Dakovo" }));
            Assert.That(sesija.Destination, Is.EqualTo("Zadar"));

            sesija.UndoLastChange();
            Assert.That(sesija.Destination, Is.EqualTo("Osijek"),
                "Ponistava se i promjena odredista, ne samo dodavanje tocaka.");

            sesija.UndoLastChange();
            Assert.That(sesija.Waypoints, Is.Empty);
        }

        [Test]
        public void PonistavanjeBezPovijesti_NeMijenjaNista()
        {
            NavigationSession sesija = Sesija("Osijek");

            Assert.That(sesija.UndoLastChange(), Is.False,
                "Nema se sto ponistiti - false, bez iznimke.");
            Assert.That(sesija.Destination, Is.EqualTo("Osijek"));
            Assert.That(sesija.Waypoints, Is.Empty);
        }

        [Test]
        public void PonistavanjePrekoPocetka_StaneNaPocetnomStanju()
        {
            NavigationSession sesija = Sesija("Osijek");
            sesija.AddWaypoint("Belisce");

            Assert.That(sesija.UndoLastChange(), Is.True);
            Assert.That(sesija.UndoLastChange(), Is.False);
            Assert.That(sesija.Waypoints, Is.Empty);
            Assert.That(sesija.Destination, Is.EqualTo("Osijek"));
        }

        [Test]
        public void PoslijePonistavanjaSeMozeNastaviti()
        {
            NavigationSession sesija = Sesija("Osijek");

            sesija.AddWaypoint("Nasice");
            sesija.AddWaypoint("Kutina");
            sesija.UndoLastChange();
            sesija.AddWaypoint("Virovitica");

            Assert.That(sesija.Waypoints, Is.EqualTo(new[] { "Nasice", "Virovitica" }));
        }

        [Test]
        public void Gradja_SkrbnikNeZnaZaTvorca()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(RouteArchive), typeof(NavigationRoute)), Is.False,
                "Arhiva cuva snimke i ne poznaje rutu. Cim bi je poznavala, mogla bi je i mijenjati.");
        }

        [Test]
        public void Gradja_TvoracNeVodiVlastituPovijest()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(NavigationRoute), typeof(RouteArchive)), Is.False);
            Assert.That(Odraz.DrziZbirkuTipa(typeof(NavigationRoute), typeof(SavedRoute)), Is.False,
                "Ruta zna snimiti i vratiti JEDNO stanje. Koliko se stanja cuva, to je posao skrbnika.");
        }

        [Test]
        public void Gradja_SnimkaSeIzvanaNeMozeMijenjati()
        {
            PropertyInfo[] pisiva = typeof(SavedRoute)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.SetMethod != null && p.SetMethod.IsPublic)
                .ToArray();

            Assert.That(pisiva, Is.Empty,
                "Snimka se izvana smije samo citati.");
        }
    }
}
