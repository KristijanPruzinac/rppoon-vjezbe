using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Promatrac.A1;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("A")]
    public class A1_Testovi
    {
        [Test]
        public void Notify_ObavjescujeSvePretplacene()
        {
            WeatherStation postaja = new WeatherStation();
            DisplayObserver zaslon = new DisplayObserver();
            LoggerObserver dnevnik = new LoggerObserver();
            postaja.Attach(zaslon);
            postaja.Attach(dnevnik);

            postaja.SetTemperature(21.5);

            Assert.That(zaslon.Last, Is.EqualTo(21.5));
            Assert.That(dnevnik.Entries, Is.EqualTo(new[] { 21.5 }));
        }

        [Test]
        public void Detach_OdjavljeniViseNeDobivaObavijesti()
        {
            // Dinamicka pretplata je razlog zasto se obrazac uopce koristi.
            WeatherStation postaja = new WeatherStation();
            DisplayObserver zaslon = new DisplayObserver();
            LoggerObserver dnevnik = new LoggerObserver();
            postaja.Attach(zaslon);
            postaja.Attach(dnevnik);
            postaja.SetTemperature(10.0);

            postaja.Detach(zaslon);
            postaja.SetTemperature(30.0);

            Assert.That(zaslon.Last, Is.EqualTo(10.0), "Odjavljeni promatrac ne smije primiti novu vrijednost.");
            Assert.That(dnevnik.Entries, Is.EqualTo(new[] { 10.0, 30.0 }));
        }

        [Test]
        public void BezPretplacenih_NotifyNePuca()
        {
            WeatherStation postaja = new WeatherStation();

            Assert.DoesNotThrow(() => postaja.SetTemperature(5.0));
            Assert.That(postaja.Temperature, Is.EqualTo(5.0));
        }

        [Test]
        public void PromjenaStanja_SamaPokreceObavjescivanje()
        {
            WeatherStation postaja = new WeatherStation();
            LoggerObserver dnevnik = new LoggerObserver();
            postaja.Attach(dnevnik);

            postaja.SetTemperature(1.0);
            postaja.SetTemperature(2.0);
            postaja.SetTemperature(3.0);

            Assert.That(dnevnik.Entries, Is.EqualTo(new[] { 1.0, 2.0, 3.0 }),
                "Svaka promjena temperature mora izazvati tocno jednu obavijest.");
        }

        [Test]
        public void NaknadnoPretplacen_DobivaSamoBuduceObavijesti()
        {
            WeatherStation postaja = new WeatherStation();
            postaja.SetTemperature(15.0);

            LoggerObserver kasni = new LoggerObserver();
            postaja.Attach(kasni);
            postaja.SetTemperature(16.0);

            Assert.That(kasni.Entries, Is.EqualTo(new[] { 16.0 }),
                "Promatrac ne dobiva povijest, samo promjene od trenutka pretplate.");
        }

        [Test]
        public void Gradja_SubjektDrziZbirkuPromatraca()
        {
            Assert.That(
                Odraz.DrziZbirkuTipa(typeof(WeatherStation), typeof(IWeatherObserver)),
                Is.True,
                "WeatherStation mora drzati zbirku tipa IWeatherObserver, a ne pojedinacne konkretne promatrace.");
        }

        [Test]
        public void Gradja_SubjektNeOvisiOKonkretnimPromatracima()
        {
            // Ako bi postaja drzala DisplayObserver ili LoggerObserver izravno,
            // dodavanje nove vrste promatraca trazilo bi izmjenu postaje.
            Assert.That(Odraz.DrziClanTipa(typeof(WeatherStation), typeof(DisplayObserver)), Is.False,
                "WeatherStation ne smije ovisiti o konkretnom razredu DisplayObserver.");
            Assert.That(Odraz.DrziClanTipa(typeof(WeatherStation), typeof(LoggerObserver)), Is.False,
                "WeatherStation ne smije ovisiti o konkretnom razredu LoggerObserver.");
        }
    }
}
