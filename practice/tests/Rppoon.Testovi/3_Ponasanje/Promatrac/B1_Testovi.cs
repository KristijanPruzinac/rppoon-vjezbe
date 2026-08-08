using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Promatrac.B1;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("B")]
    public class B1_Testovi
    {
        [Test]
        public void Measure_RastucaVrijednost_ObavjescujePratitelje()
        {
            EarthquakeDetector detektor = new EarthquakeDetector();
            Logger dnevnik = new Logger();
            detektor.StartWatching(dnevnik);

            detektor.Measure(2.5);

            Assert.That(dnevnik.Count, Is.EqualTo(1));
            Assert.That(dnevnik.LastValue, Is.EqualTo(2.5));
        }

        [Test]
        public void Measure_PadajucaVrijednost_NeObavjescuje()
        {
            // Uvjet zadatka: obavijest ide samo kad vrijednost raste.
            EarthquakeDetector detektor = new EarthquakeDetector();
            Logger dnevnik = new Logger();
            detektor.StartWatching(dnevnik);
            detektor.Measure(5.0);

            detektor.Measure(3.0);

            Assert.That(dnevnik.Count, Is.EqualTo(1), "Pad vrijednosti ne smije izazvati obavijest.");
            Assert.That(detektor.SensorValue, Is.EqualTo(3.0), "Stanje se svejedno azurira.");
        }

        [Test]
        public void Measure_IstaVrijednost_NeObavjescuje()
        {
            EarthquakeDetector detektor = new EarthquakeDetector();
            Logger dnevnik = new Logger();
            detektor.StartWatching(dnevnik);
            detektor.Measure(4.0);

            detektor.Measure(4.0);

            Assert.That(dnevnik.Count, Is.EqualTo(1), "Nepromijenjena vrijednost nije rast.");
        }

        [Test]
        public void Alarm_BrojiSamoVrijednostiIznadPraga()
        {
            EarthquakeDetector detektor = new EarthquakeDetector();
            Alarm alarm = new Alarm(5.0);
            detektor.StartWatching(alarm);

            detektor.Measure(3.0);
            detektor.Measure(5.0);
            detektor.Measure(7.0);

            Assert.That(alarm.Triggered, Is.EqualTo(1),
                "Prag je strog: samo 7.0 je iznad 5.0.");
        }

        [Test]
        public void StopWatching_PrekidaObavjescivanje()
        {
            EarthquakeDetector detektor = new EarthquakeDetector();
            Logger dnevnik = new Logger();
            detektor.StartWatching(dnevnik);
            detektor.Measure(1.0);

            detektor.StopWatching(dnevnik);
            detektor.Measure(9.0);

            Assert.That(dnevnik.Count, Is.EqualTo(1));
        }

        [Test]
        public void ViseRaznorodnihPratitelja_SvakiReagiraNaSvoj()
        {
            EarthquakeDetector detektor = new EarthquakeDetector();
            Logger dnevnik = new Logger();
            Alarm alarm = new Alarm(6.0);
            detektor.StartWatching(dnevnik);
            detektor.StartWatching(alarm);

            detektor.Measure(4.0);
            detektor.Measure(8.0);

            Assert.That(dnevnik.Count, Is.EqualTo(2), "Dnevnik biljezi svaku obavijest.");
            Assert.That(alarm.Triggered, Is.EqualTo(1), "Alarm reagira samo iznad praga.");
        }

        [Test]
        public void Gradja_DetektorDrziZbirkuPratitelja()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(EarthquakeDetector), typeof(IWatcher)), Is.True,
                "EarthquakeDetector mora drzati zbirku tipa IWatcher.");
        }

        [Test]
        public void Gradja_DetektorNeOvisiOKonkretnimPratiteljima()
        {
            Assert.That(Odraz.DrziClanTipa(typeof(EarthquakeDetector), typeof(Logger)), Is.False);
            Assert.That(Odraz.DrziClanTipa(typeof(EarthquakeDetector), typeof(Alarm)), Is.False);
        }
    }
}
