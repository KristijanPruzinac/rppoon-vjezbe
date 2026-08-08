using NUnit.Framework;
using Rppoon.Testovi.Alati;
using Rppoon.Zadaci.Promatrac.B2;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("B")]
    public class B2_Testovi
    {
        [Test]
        public void SetFlow_ObavjescujePretplacene()
        {
            FlowSensor mjerac = new FlowSensor("Zapad");
            FlowRecorder zapisnik = new FlowRecorder();
            mjerac.Subscribe(zapisnik);

            mjerac.SetFlow(12.5);

            Assert.That(zapisnik.Received, Is.EqualTo(1));
            Assert.That(mjerac.Flow, Is.EqualTo(12.5));
        }

        [Test]
        public void Obavijest_NosiImeMjeraca()
        {
            // "Push" nacin: subjekt salje podatke uz obavijest, pa pratitelj
            // ne mora natrag pitati tko ga je i zasto obavijestio.
            FlowSensor mjerac = new FlowSensor("Zapad");
            MailNotifier posta = new MailNotifier();
            mjerac.Subscribe(posta);

            mjerac.SetFlow(12.5);

            Assert.That(posta.LastMessage, Is.EqualTo("Zapad: 12.5"));
        }

        [Test]
        public void IstiPratitelj_MozeSlusatiDvaMjeraca()
        {
            // Ovo je razlog zasto pratitelj ne smije biti vezan uz jedan subjekt.
            FlowSensor zapad = new FlowSensor("Zapad");
            FlowSensor istok = new FlowSensor("Istok");
            FlowRecorder zapisnik = new FlowRecorder();
            zapad.Subscribe(zapisnik);
            istok.Subscribe(zapisnik);

            zapad.SetFlow(1.0);
            istok.SetFlow(2.0);

            Assert.That(zapisnik.Received, Is.EqualTo(2));
        }

        [Test]
        public void OdjavaSJednogMjeraca_NeUtjeceNaDrugi()
        {
            FlowSensor zapad = new FlowSensor("Zapad");
            FlowSensor istok = new FlowSensor("Istok");
            MailNotifier posta = new MailNotifier();
            zapad.Subscribe(posta);
            istok.Subscribe(posta);

            zapad.Unsubscribe(posta);
            zapad.SetFlow(9.0);
            istok.SetFlow(3.0);

            Assert.That(posta.LastMessage, Is.EqualTo("Istok: 3"),
                "Odjava sa zapadnog mjeraca ne smije prekinuti pretplatu na istocni.");
        }

        [Test]
        public void ViseIzmjena_SvakaSaljeObavijest()
        {
            FlowSensor mjerac = new FlowSensor("Sjever");
            FlowRecorder zapisnik = new FlowRecorder();
            mjerac.Subscribe(zapisnik);

            mjerac.SetFlow(1.0);
            mjerac.SetFlow(1.0);
            mjerac.SetFlow(2.0);

            Assert.That(zapisnik.Received, Is.EqualTo(3),
                "Ovdje se obavjescuje svaka izmjena, bez uvjeta na rast.");
        }

        [Test]
        public void BezPretplacenih_SetFlowNePuca()
        {
            FlowSensor mjerac = new FlowSensor("Jug");

            Assert.DoesNotThrow(() => mjerac.SetFlow(4.0));
            Assert.That(mjerac.Flow, Is.EqualTo(4.0));
        }

        [Test]
        public void Gradja_MjeracDrziZbirkuPratitelja()
        {
            Assert.That(Odraz.DrziZbirkuTipa(typeof(FlowSensor), typeof(IFlowObserver)), Is.True);
        }

        [Test]
        public void Gradja_PratiteljNeDrziReferencuNaSubjekt()
        {
            // Ovisnost ide samo u jednom smjeru: subjekt -> pratitelj.
            Assert.That(Odraz.DrziClanTipa(typeof(MailNotifier), typeof(FlowSensor)), Is.False,
                "Pratitelj ne smije drzati referencu na konkretan mjerac.");
            Assert.That(Odraz.DrziClanTipa(typeof(FlowRecorder), typeof(FlowSensor)), Is.False);
        }
    }
}
