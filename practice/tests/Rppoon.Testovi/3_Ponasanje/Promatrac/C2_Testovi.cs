using System;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Promatrac
{
    [TestFixture]
    [Category("Ponasanje")]
    [Category("Promatrac")]
    [Category("C")]
    public class C2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Promatrac.C2";

        private static object Mreza() => Odraz.Novi(Odraz.Tip(Ns, "EnergyGrid"));

        private static object Dnevnik() => Odraz.Novi(Odraz.Tip(Ns, "EnergyLogger"));

        private static object Rezerva() => Odraz.Novi(Odraz.Tip(Ns, "BackupUnit"));

        [Test]
        public void SvakaIzmjena_SaljeObavijest()
        {
            object mreza = Mreza();
            object dnevnik = Dnevnik();
            Odraz.Pozovi(mreza, "Attach", dnevnik);

            Odraz.Pozovi(mreza, "SetProduction", 100.0);
            Odraz.Pozovi(mreza, "SetConsumption", 40.0);

            Assert.That(Odraz.Pozovi(dnevnik, "Entries"), Is.EqualTo(2),
                "I proizvodnja i potrosnja moraju izazvati obavijest.");
            Assert.That(Odraz.Pozovi(dnevnik, "LastProduction"), Is.EqualTo(100.0));
            Assert.That(Odraz.Pozovi(dnevnik, "LastConsumption"), Is.EqualTo(40.0));
        }

        [Test]
        public void Rezerva_AktivirasSeNaDevedesetPosto()
        {
            object mreza = Mreza();
            object rezerva = Rezerva();
            Odraz.Pozovi(mreza, "Attach", rezerva);

            Odraz.Pozovi(mreza, "SetProduction", 100.0);
            Odraz.Pozovi(mreza, "SetConsumption", 90.0);

            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.True, "Granica od 90 % je ukljuciva.");
        }

        [Test]
        public void Rezerva_IspodGranice_NijeAktivna()
        {
            object mreza = Mreza();
            object rezerva = Rezerva();
            Odraz.Pozovi(mreza, "Attach", rezerva);

            Odraz.Pozovi(mreza, "SetProduction", 100.0);
            Odraz.Pozovi(mreza, "SetConsumption", 89.9);

            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.False);
        }

        [Test]
        public void Rezerva_NultaProizvodnja_NeAktivirasSe()
        {
            // Bez ovog uvjeta rjesenje dijeli nulom ili uvijek okida.
            object mreza = Mreza();
            object rezerva = Rezerva();
            Odraz.Pozovi(mreza, "Attach", rezerva);

            Odraz.Pozovi(mreza, "SetConsumption", 50.0);

            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.False,
                "Uz nultu proizvodnju nema se sto usporediti.");
        }

        [Test]
        public void Rezerva_PovratakIspodGranice_Deaktivira()
        {
            object mreza = Mreza();
            object rezerva = Rezerva();
            Odraz.Pozovi(mreza, "Attach", rezerva);
            Odraz.Pozovi(mreza, "SetProduction", 100.0);
            Odraz.Pozovi(mreza, "SetConsumption", 95.0);
            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.True);

            Odraz.Pozovi(mreza, "SetConsumption", 20.0);

            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.False);
            Assert.That(Odraz.Pozovi(rezerva, "Activations"), Is.EqualTo(1));
        }

        [Test]
        public void Detach_PrekidaSinkronizaciju()
        {
            object mreza = Mreza();
            object dnevnik = Dnevnik();
            Odraz.Pozovi(mreza, "Attach", dnevnik);
            Odraz.Pozovi(mreza, "SetProduction", 10.0);

            Odraz.Pozovi(mreza, "Detach", dnevnik);
            Odraz.Pozovi(mreza, "SetProduction", 20.0);

            Assert.That(Odraz.Pozovi(dnevnik, "Entries"), Is.EqualTo(1));
        }

        [Test]
        public void RaznorodneKomponente_ReagirajuNaIstuObavijest()
        {
            object mreza = Mreza();
            object dnevnik = Dnevnik();
            object rezerva = Rezerva();
            Odraz.Pozovi(mreza, "Attach", dnevnik);
            Odraz.Pozovi(mreza, "Attach", rezerva);

            Odraz.Pozovi(mreza, "SetProduction", 100.0);
            Odraz.Pozovi(mreza, "SetConsumption", 95.0);

            Assert.That(Odraz.Pozovi(dnevnik, "Entries"), Is.EqualTo(2));
            Assert.That(Odraz.Pozovi(rezerva, "Activated"), Is.True);
        }

        [Test]
        public void Gradja_MrezaDrziZbirkuPromatraca()
        {
            Assert.That(
                Odraz.DrziZbirkuTipa(Odraz.Tip(Ns, "EnergyGrid"), Odraz.Sucelje(Ns, "IEnergyObserver")),
                Is.True);
        }

        [Test]
        public void Gradja_MrezaNeOvisiOKonkretnimKomponentama()
        {
            // Zadatak trazi da se nove komponente dodaju bez izmjene klijentskog koda.
            Type mreza = Odraz.Tip(Ns, "EnergyGrid");

            Assert.That(Odraz.DrziClanTipa(mreza, Odraz.Tip(Ns, "EnergyLogger")), Is.False);
            Assert.That(Odraz.DrziClanTipa(mreza, Odraz.Tip(Ns, "BackupUnit")), Is.False);
        }
    }
}
