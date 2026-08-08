using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Memento.B1;

namespace Rppoon.Testovi.Memento
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Memento")]
    [Category("B")]
    public class B1_Testovi
    {
        [Test]
        public void NovoPlatnoJePrazno()
        {
            Assert.That(new DrawingScreen().Shapes, Is.Empty);
        }

        [Test]
        public void CrtanjeDodajeOblikeRedom()
        {
            DrawingScreen platno = new DrawingScreen();

            platno.Draw(new Shape("krug"));
            platno.Draw(new Shape("pravokutnik"));

            Assert.That(platno.Shapes, Is.EqualTo(new[] { "krug", "pravokutnik" }));
        }

        [Test]
        public void SnimkaNeVidiObliceNacrtaneNAKONNje()
        {
            DrawingScreen platno = new DrawingScreen();
            platno.Draw(new Shape("krug"));
            ScreenState snimka = platno.Save();

            platno.Draw(new Shape("linija"));
            platno.Draw(new Shape("pravokutnik"));
            platno.Restore(snimka);

            Assert.That(platno.Shapes, Is.EqualTo(new[] { "krug" }),
                "Stanje platna je LISTA. Ako snimka zapamti onu istu listu koju platno " +
                "i dalje mijenja, snimka se mijenja zajedno s njim i vracanje ne vrati nista. " +
                "Snimka mora imati svoju kopiju.");
        }

        [Test]
        public void PlatnoSeMozeVratitiNaBiloKojeRanijeStanje()
        {
            DrawingScreen platno = new DrawingScreen();
            platno.Draw(new Shape("krug"));
            ScreenState prvo = platno.Save();
            platno.Draw(new Shape("linija"));
            ScreenState drugo = platno.Save();
            platno.Draw(new Shape("pravokutnik"));

            platno.Restore(prvo);
            Assert.That(platno.Shapes, Is.EqualTo(new[] { "krug" }));

            platno.Restore(drugo);
            Assert.That(platno.Shapes, Is.EqualTo(new[] { "krug", "linija" }),
                "Snimke se ne trose - svaka i dalje vrijedi.");
        }

        [Test]
        public void NakonVracanjaSeMozeNastavitiCrtati()
        {
            DrawingScreen platno = new DrawingScreen();
            ScreenState prazno = platno.Save();
            platno.Draw(new Shape("krug"));

            platno.Restore(prazno);
            platno.Draw(new Shape("linija"));

            Assert.That(platno.Shapes, Is.EqualTo(new[] { "linija" }));
        }

        [Test]
        public void PovijestVracaStanjaObrnutimRedom()
        {
            DrawingScreen platno = new DrawingScreen();
            DrawingHistory povijest = new DrawingHistory();

            povijest.Push(platno.Save());
            platno.Draw(new Shape("krug"));
            povijest.Push(platno.Save());

            Assert.That(povijest.Count, Is.EqualTo(2));

            platno.Restore(povijest.Pop());
            Assert.That(platno.Shapes, Has.Count.EqualTo(1));

            platno.Restore(povijest.Pop());
            Assert.That(platno.Shapes, Is.Empty);

            Assert.That(povijest.Count, Is.EqualTo(0));
            Assert.That(povijest.Pop(), Is.Null, "Iz prazne povijesti izlazi null, ne iznimka.");
        }

        [Test]
        public void PovijestCuvaNajviseDesetStanja()
        {
            DrawingScreen platno = new DrawingScreen();
            DrawingHistory povijest = new DrawingHistory();

            for (int i = 0; i < 12; i++)
            {
                povijest.Push(platno.Save());
                platno.Draw(new Shape("oblik" + i));
            }

            Assert.That(povijest.Count, Is.EqualTo(DrawingHistory.Kapacitet),
                "Zadatak trazi cuvanje zadnjih deset stanja.");
        }

        [Test]
        public void PrekoracenjeKapaciteta_IspadaNAJSTARIJEStanje()
        {
            DrawingScreen platno = new DrawingScreen();
            DrawingHistory povijest = new DrawingHistory();

            // Spremljena stanja imaju redom 0, 1, 2, ... 11 oblika.
            for (int i = 0; i < 12; i++)
            {
                povijest.Push(platno.Save());
                platno.Draw(new Shape("oblik" + i));
            }

            platno.Restore(povijest.Pop());
            Assert.That(platno.Shapes, Has.Count.EqualTo(11),
                "Zadnje spremljeno stanje mora ostati.");

            ScreenState najstarijeSacuvano = null;
            for (int i = 0; i < DrawingHistory.Kapacitet - 1; i++)
            {
                najstarijeSacuvano = povijest.Pop();
            }

            platno.Restore(najstarijeSacuvano);
            Assert.That(platno.Shapes, Has.Count.EqualTo(2),
                "Stanja s 0 i 1 oblikom su ispala jer su najstarija. " +
                "Ako ovdje ostane 0, izbacivalo se s krivog kraja.");
            Assert.That(povijest.Count, Is.EqualTo(0));
        }

        [Test]
        public void AlatPonistavaZadnjiNacrtaniOblik()
        {
            DrawingApp alat = new DrawingApp();

            alat.Add("krug");
            alat.Add("linija");
            alat.Add("pravokutnik");

            Assert.That(alat.Undo(), Is.True);

            Assert.That(alat.Shapes, Is.EqualTo(new[] { "krug", "linija" }));
        }

        [Test]
        public void AlatPonistavaSveDoPraznogPlatna()
        {
            DrawingApp alat = new DrawingApp();
            alat.Add("krug");
            alat.Add("linija");

            alat.Undo();
            alat.Undo();

            Assert.That(alat.Shapes, Is.Empty);
            Assert.That(alat.Undo(), Is.False, "Nema se sto ponistiti.");
        }

        [Test]
        public void AlatNaPraznomPlatnu_UndoNeRadiNista()
        {
            DrawingApp alat = new DrawingApp();

            Assert.That(alat.Undo(), Is.False);
            Assert.That(alat.Shapes, Is.Empty);
        }

        [Test]
        public void AlatPamtiSamoZadnjihDesetRadnji()
        {
            DrawingApp alat = new DrawingApp();
            for (int i = 0; i < 12; i++)
            {
                alat.Add("oblik" + i);
            }

            for (int i = 0; i < DrawingHistory.Kapacitet; i++)
            {
                Assert.That(alat.Undo(), Is.True, "Deset ponistavanja mora proci.");
            }

            Assert.That(alat.Undo(), Is.False, "Jedanaesto ponistavanje vise nema sto vratiti.");
            Assert.That(alat.Shapes, Has.Count.EqualTo(2),
                "Dvanaest crtanja, deset ponistavanja - ostaju prva dva oblika.");
        }

        [Test]
        public void NakonPonistavanjaSeMozeNastavitiCrtati()
        {
            DrawingApp alat = new DrawingApp();
            alat.Add("krug");
            alat.Add("linija");

            alat.Undo();
            alat.Add("pravokutnik");

            Assert.That(alat.Shapes, Is.EqualTo(new[] { "krug", "pravokutnik" }));
        }

        [Test]
        public void Gradja_ZbirkaOblikaNijeJavnoDostupna()
        {
            Type[] javniTipoviClanova = JavniClanovi(typeof(DrawingScreen)).ToArray();

            Assert.That(javniTipoviClanova.Any(t => ZbirkaOblika(t)), Is.False,
                "Zadatak izricito trazi da oblici NISU javno dostupni. " +
                "Javna zbirka Shape-ova (ili lista koja se moze mijenjati) to krsi - " +
                "tko god je dohvati moze zaobici i platno i povijest.");
        }

        [Test]
        public void Gradja_TvoracNeVodiVlastituPovijest()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(DrawingScreen), typeof(DrawingHistory)), Is.False);
            Assert.That(Odraz.DrziZbirkuTipa(typeof(DrawingScreen), typeof(ScreenState)), Is.False,
                "Platno zna snimiti i vratiti JEDNO stanje. Koliko ih se cuva, to je posao skrbnika.");
        }

        [Test]
        public void Gradja_SkrbnikNeZnaZaTvorca()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(DrawingHistory), typeof(DrawingScreen)), Is.False,
                "Povijest cuva snimke i ne poznaje platno.");
            Assert.That(Odraz.DrziZbirkuTipa(typeof(DrawingHistory), typeof(Shape)), Is.False,
                "Povijest ne rastavlja snimke na oblike - njoj je snimka zatvorena kutija.");
        }

        [Test]
        public void Gradja_SnimkaSeIzvanaNeMozeCitatiNiMijenjati()
        {
            MemberInfo[] javni = typeof(ScreenState)
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                .ToArray();

            Assert.That(javni, Is.Empty,
                "Snimku otvara samo platno. Zato njezino stanje ne smije biti javno - " +
                "u C#-u se to postize modifikatorom 'internal' (isti sklop) ili ugnijezdenim razredom.");
        }

        // ---- pomagala ----

        private static IEnumerable<Type> JavniClanovi(Type domacin)
        {
            const BindingFlags gdje = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            foreach (FieldInfo polje in domacin.GetFields(gdje))
            {
                yield return polje.FieldType;
            }

            foreach (PropertyInfo svojstvo in domacin.GetProperties(gdje))
            {
                yield return svojstvo.PropertyType;
            }
        }

        private static bool ZbirkaOblika(Type tip)
        {
            if (tip == typeof(string))
            {
                return false;
            }

            if (tip.IsArray)
            {
                return typeof(Shape).IsAssignableFrom(tip.GetElementType());
            }

            bool nosiOblike = tip.GetInterfaces().Concat(new[] { tip })
                .Any(s => s.IsGenericType
                          && s.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                          && typeof(Shape).IsAssignableFrom(s.GetGenericArguments()[0]));

            if (nosiOblike)
            {
                return true;
            }

            // Popis vrsta smije biti javan, ali samo ako se ne moze mijenjati.
            return tip.GetInterfaces().Concat(new[] { tip })
                .Any(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof(ICollection<>))
                && typeof(IEnumerable<string>).IsAssignableFrom(tip);
        }
    }
}
