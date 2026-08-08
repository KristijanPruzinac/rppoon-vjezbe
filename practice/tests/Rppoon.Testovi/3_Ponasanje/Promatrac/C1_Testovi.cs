using System;
using System.Linq;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("C")]
    public class C1_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Promatrac.C1";

        private static object MooTuber(string kanal)
        {
            return Odraz.Novi(Odraz.Tip(Ns, "MooTuber"), kanal);
        }

        private static object Gledatelj()
        {
            return Odraz.Novi(Odraz.Tip(Ns, "Viewer"));
        }

        [Test]
        public void Publish_ObavjescujePretplacenog()
        {
            object kreator = MooTuber("KravaTV");
            object gledatelj = Gledatelj();
            Odraz.Pozovi(kreator, "Subscribe", gledatelj);

            Odraz.Pozovi(kreator, "Publish", "https://moo.tube/1");

            Assert.That(Odraz.Pozovi(gledatelj, "LastUrl"), Is.EqualTo("https://moo.tube/1"));
            Assert.That(Odraz.Pozovi(gledatelj, "NotificationCount"), Is.EqualTo(1));
        }

        [Test]
        public void Publish_ObavjescujeSvePretplacene()
        {
            object kreator = MooTuber("KravaTV");
            object prvi = Gledatelj();
            object drugi = Gledatelj();
            Odraz.Pozovi(kreator, "Subscribe", prvi);
            Odraz.Pozovi(kreator, "Subscribe", drugi);

            Odraz.Pozovi(kreator, "Publish", "https://moo.tube/2");

            Assert.That(Odraz.Pozovi(prvi, "NotificationCount"), Is.EqualTo(1));
            Assert.That(Odraz.Pozovi(drugi, "NotificationCount"), Is.EqualTo(1));
        }

        [Test]
        public void Unsubscribe_PrekidaPracenje()
        {
            object kreator = MooTuber("KravaTV");
            object gledatelj = Gledatelj();
            Odraz.Pozovi(kreator, "Subscribe", gledatelj);
            Odraz.Pozovi(kreator, "Publish", "https://moo.tube/1");

            Odraz.Pozovi(kreator, "Unsubscribe", gledatelj);
            Odraz.Pozovi(kreator, "Publish", "https://moo.tube/2");

            Assert.That(Odraz.Pozovi(gledatelj, "NotificationCount"), Is.EqualTo(1),
                "Nakon odjave pracenje prestaje.");
            Assert.That(Odraz.Pozovi(gledatelj, "LastUrl"), Is.EqualTo("https://moo.tube/1"));
        }

        [Test]
        public void IstiGledatelj_MozePratitiViseKreatora()
        {
            object prvi = MooTuber("KravaTV");
            object drugi = MooTuber("MuuVlog");
            object gledatelj = Gledatelj();
            Odraz.Pozovi(prvi, "Subscribe", gledatelj);
            Odraz.Pozovi(drugi, "Subscribe", gledatelj);

            Odraz.Pozovi(prvi, "Publish", "https://moo.tube/a");
            Odraz.Pozovi(drugi, "Publish", "https://moo.tube/b");

            Assert.That(Odraz.Pozovi(gledatelj, "NotificationCount"), Is.EqualTo(2));
            Assert.That(Odraz.Pozovi(gledatelj, "LastUrl"), Is.EqualTo("https://moo.tube/b"));
        }

        [Test]
        public void BezPretplacenih_ObjavaNePuca()
        {
            object kreator = MooTuber("Prazan");

            Assert.DoesNotThrow(() => Odraz.Pozovi(kreator, "Publish", "https://moo.tube/x"));
        }

        [Test]
        public void VelikBrojPratitelja_SviDobivajuObavijest()
        {
            // Zadatak izricito trazi da velik broj korisnika moze pratiti.
            object kreator = MooTuber("KravaTV");
            object[] gledatelji = Enumerable.Range(0, 500).Select(i => Gledatelj()).ToArray();
            foreach (object g in gledatelji)
            {
                Odraz.Pozovi(kreator, "Subscribe", g);
            }

            Odraz.Pozovi(kreator, "Publish", "https://moo.tube/masovno");

            Assert.That(gledatelji.All(g => (int)Odraz.Pozovi(g, "NotificationCount") == 1), Is.True);
        }

        [Test]
        public void Gradja_KreatorDrziZbirkuPretplatnika()
        {
            Assert.That(
                Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "MooTuber"), Odraz.Sucelje(Ns, "ISubscriber")),
                Is.True,
                "MooTuber mora drzati zbirku tipa ISubscriber.");
        }

        [Test]
        public void Gradja_KreatorNeOvisiOKonkretnomGledatelju()
        {
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "MooTuber"), Odraz.Tip(Ns, "Viewer")),
                Is.False,
                "MooTuber ne smije ovisiti o razredu Viewer - inace nova vrsta pratitelja trazi izmjenu MooTubera.");
        }

        [Test]
        public void Gradja_ViewerImplementiraSucelje()
        {
            Assert.That(Odraz.Sucelje(Ns, "ISubscriber").IsAssignableFrom(Odraz.Tip(Ns, "Viewer")), Is.True);
        }
    }
}
