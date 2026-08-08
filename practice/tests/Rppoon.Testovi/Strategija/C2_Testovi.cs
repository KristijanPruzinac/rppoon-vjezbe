using System;
using System.Linq;
using NUnit.Framework;
using Rppoon.Testovi.Alati;

namespace Rppoon.Testovi.Strategija
{
    [TestFixture]
    [Category("Strategija")]
    [Category("C")]
    public class C2_Testovi
    {
        private const string Ns = "Rppoon.Zadaci.Strategija.C2";

        private static bool Provjeri(string nazivStrategije, string unos, params object[] argumentiStrategije)
        {
            object strategija = Odraz.Novi(Odraz.Tip(Ns, nazivStrategije), argumentiStrategije);
            object obrazac = Odraz.Novi(Odraz.Tip(Ns, "RegistrationForm"), strategija);
            return (bool)Odraz.Pozovi(obrazac, "Check", unos);
        }

        [Test]
        public void NotEmptyValidation_OdbijaPrazanIRazmake()
        {
            Assert.That(Provjeri("NotEmptyValidation", "Ana"), Is.True);
            Assert.That(Provjeri("NotEmptyValidation", ""), Is.False);
            Assert.That(Provjeri("NotEmptyValidation", "   "), Is.False, "Sami razmaci nisu ispravan unos.");
            Assert.That(Provjeri("NotEmptyValidation", null), Is.False);
        }

        [Test]
        public void MinLengthValidation_PostujeZadanuNajmanjuDuljinu()
        {
            Assert.That(Provjeri("MinLengthValidation", "lozinka123", 8), Is.True);
            Assert.That(Provjeri("MinLengthValidation", "kratko", 8), Is.False);
            Assert.That(Provjeri("MinLengthValidation", "tocno8zn", 8), Is.True, "Granica je ukljuciva.");
        }

        [Test]
        public void DigitRequiredValidation_TraziBaremJednuZnamenku()
        {
            Assert.That(Provjeri("DigitRequiredValidation", "lozinka1"), Is.True);
            Assert.That(Provjeri("DigitRequiredValidation", "lozinka"), Is.False);
        }

        [Test]
        public void ZamjenaPravilaUHodu_MijenjaOdluku()
        {
            object obrazac = Odraz.Novi(
                Odraz.Tip(Ns, "RegistrationForm"),
                Odraz.Novi(Odraz.Tip(Ns, "NotEmptyValidation")));

            Assert.That(Odraz.Pozovi(obrazac, "Check", "abc"), Is.True);

            Odraz.Postavi(obrazac, "Strategy",
                Odraz.Novi(Odraz.Tip(Ns, "MinLengthValidation"), 8));

            Assert.That(Odraz.Pozovi(obrazac, "Check", "abc"), Is.False,
                "Isti obrazac, drugo pravilo, druga odluka.");
        }

        [Test]
        public void Gradja_SveTriPravilaImplementirajuSucelje()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IValidationStrategy");

            foreach (string naziv in new[] { "NotEmptyValidation", "MinLengthValidation", "DigitRequiredValidation" })
            {
                Assert.That(sucelje.IsAssignableFrom(Odraz.Tip(Ns, naziv)), Is.True,
                    $"'{naziv}' mora implementirati IValidationStrategy.");
            }
        }

        [Test]
        public void Gradja_SuceljeImaTocnoJednuMetoduIsValid()
        {
            Type sucelje = Odraz.Sucelje(Ns, "IValidationStrategy");
            var metode = sucelje.GetMethods();

            Assert.That(metode.Length, Is.EqualTo(1), "Strategija ima usko sucelje - tocno jednu operaciju.");
            Assert.That(metode[0].Name, Is.EqualTo("IsValid"));
            Assert.That(metode[0].ReturnType, Is.EqualTo(typeof(bool)));
            Assert.That(metode[0].GetParameters().Select(p => p.ParameterType),
                Is.EqualTo(new[] { typeof(string) }));
        }

        [Test]
        public void Gradja_FormaDrziReferencuNaSucelje()
        {
            Assert.That(
                Odraz.DrziClanTipa(Odraz.Tip(Ns, "RegistrationForm"), Odraz.Sucelje(Ns, "IValidationStrategy")),
                Is.True,
                "RegistrationForm mora drzati clan tipa IValidationStrategy, a ne provjeravati pravila sam.");
        }
    }
}
