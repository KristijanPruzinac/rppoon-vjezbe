using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Rppoon.Testovi.Alati
{
    /// <summary>
    /// Pomagala za provjeru STRUKTURE obrasca, a ne samo ispisa.
    ///
    /// Zasto: zadatak "sortiraj rastuce" mozes rijesiti s jednim list.Sort()
    /// i test bi prosao — a obrazac Strategija ne bi bio primijenjen. Zato
    /// testovi uz ponasanje provjeravaju i gradju: postoji li sucelje, drzi
    /// li kontekst referencu na njega, je li konstruktor skriven, i slicno.
    /// </summary>
    public static class Odraz
    {
        public static Assembly Sklop => typeof(Rppoon.Zadaci.Sidro).Assembly;

        private static Type[] SviTipovi()
        {
            try
            {
                return Sklop.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToArray();
            }
        }

        // ---------- dohvat tipova ----------

        /// <summary>Svi tipovi deklarirani tocno u zadanom prostoru imena.</summary>
        public static IReadOnlyList<Type> TipoviU(string prostorImena)
            => SviTipovi().Where(t => t.Namespace == prostorImena).ToList();

        /// <summary>
        /// Dohvaca tip po imenu; ako ne postoji, test pada s porukom koja kaze
        /// sto tocno nedostaje (a ne s NullReferenceException).
        /// </summary>
        public static Type Tip(string prostorImena, string naziv)
        {
            Type tip = TipoviU(prostorImena).FirstOrDefault(t => t.Name == naziv);
            if (tip == null)
            {
                IEnumerable<string> nadeni = TipoviU(prostorImena).Select(t => t.Name).OrderBy(n => n);
                Assert.Fail(
                    $"Nedostaje javni tip '{naziv}' u prostoru imena '{prostorImena}'.\n" +
                    $"Pronadeni tipovi: {(nadeni.Any() ? string.Join(", ", nadeni) : "(nijedan)")}\n" +
                    "Podsjetnik: tip mora biti 'public' da bi ga testni projekt vidio.");
            }
            return tip;
        }

        /// <summary>Sucelje u prostoru imena; pada s jasnom porukom ako ga nema.</summary>
        public static Type Sucelje(string prostorImena, string naziv)
        {
            Type tip = Tip(prostorImena, naziv);
            Assert.That(tip.IsInterface, Is.True, $"'{naziv}' mora biti sucelje (interface).");
            return tip;
        }

        /// <summary>Konkretni razredi u sklopu koji implementiraju zadano sucelje.</summary>
        public static IReadOnlyList<Type> Implementacije(Type sucelje)
            => SviTipovi()
                .Where(t => t.IsClass && !t.IsAbstract && t != sucelje && sucelje.IsAssignableFrom(t))
                .ToList();

        /// <summary>Konkretni razredi koji nasljeduju zadani bazni razred.</summary>
        public static IReadOnlyList<Type> Nasljednici(Type bazni)
            => SviTipovi()
                .Where(t => t.IsClass && !t.IsAbstract && t != bazni && bazni.IsAssignableFrom(t))
                .ToList();

        // ---------- provjera gradje ----------

        /// <summary>
        /// Ima li <paramref name="domacin"/> polje ili svojstvo tipa
        /// <paramref name="trazeni"/>? Time se dokazuje KOMPOZICIJA — npr. da
        /// dekorater omata drugi objekt, a ne da ga nasljeduje.
        /// </summary>
        public static bool DrziClanTipa(Type domacin, Type trazeni)
            => ClanoviTipa(domacin).Any(t => trazeni.IsAssignableFrom(t));

        /// <summary>
        /// Ima li <paramref name="domacin"/> zbirku (IEnumerable&lt;T&gt;) ciji je
        /// element <paramref name="trazeni"/>? Za Kompozit i Promatrac.
        /// </summary>
        public static bool DrziZbirkuTipa(Type domacin, Type trazeni)
            => ClanoviTipa(domacin).Any(t => ElementZbirke(t) is Type e && trazeni.IsAssignableFrom(e));

        private static IEnumerable<Type> ClanoviTipa(Type domacin)
        {
            const BindingFlags gdje = BindingFlags.Instance | BindingFlags.Static
                                    | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (FieldInfo polje in domacin.GetFields(gdje))
                yield return polje.FieldType;

            foreach (PropertyInfo svojstvo in domacin.GetProperties(gdje))
                yield return svojstvo.PropertyType;
        }

        private static Type ElementZbirke(Type tip)
        {
            if (tip.IsArray) return tip.GetElementType();

            foreach (Type sucelje in tip.GetInterfaces().Concat(new[] { tip }))
            {
                if (sucelje.IsGenericType && sucelje.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return sucelje.GetGenericArguments()[0];
            }
            return null;
        }

        /// <summary>Nijedan konstruktor nije javan — nuzno za Singleton.</summary>
        public static bool NemaJavnogKonstruktora(Type tip)
            => tip.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0;

        // ---------- poziv bez prevodenja (za ispitnu razinu C) ----------

        /// <summary>Stvara primjerak tipa preko konstruktora koji odgovara argumentima.</summary>
        public static object Novi(Type tip, params object[] argumenti)
        {
            try
            {
                return Activator.CreateInstance(tip, argumenti);
            }
            catch (MissingMethodException)
            {
                string potpisi = string.Join("\n  ", tip.GetConstructors().Select(PotpisKonstruktora));
                Assert.Fail(
                    $"Razred '{tip.Name}' nema konstruktor koji prima ({OpisArgumenata(argumenti)}).\n" +
                    $"Dostupni konstruktori:\n  {(potpisi.Length > 0 ? potpisi : "(samo bez parametara ili nijedan javni)")}");
                return null;
            }
        }

        /// <summary>
        /// Poziva metodu po imenu. Ako metode nema, a postoji svojstvo istog
        /// imena bez parametara, uzima njega.
        ///
        /// Zasto popustljivo: "Total()" i "Total { get; }" jednako dobro
        /// rjesavaju zadatak. Test smije traziti REZULTAT, ali ne smije
        /// obarati studenta zbog izbora koji zadatak nije propisao.
        /// </summary>
        public static object Pozovi(object primjerak, string nazivMetode, params object[] argumenti)
        {
            Type tip = primjerak.GetType();
            MethodInfo metoda = tip.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                   .FirstOrDefault(m => m.Name == nazivMetode &&
                                                        m.GetParameters().Length == argumenti.Length);

            if (metoda == null && argumenti.Length == 0)
            {
                PropertyInfo svojstvo = tip.GetProperty(nazivMetode, BindingFlags.Instance | BindingFlags.Public);
                if (svojstvo != null && svojstvo.CanRead)
                {
                    metoda = svojstvo.GetGetMethod();
                }
            }

            if (metoda == null)
            {
                string imena = string.Join(", ", tip.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                    .Where(m => !m.IsSpecialName)
                                                    .Select(m => m.Name).Distinct().OrderBy(n => n));
                Assert.Fail(
                    $"Razred '{tip.Name}' nema javnu metodu '{nazivMetode}' s {argumenti.Length} parametara.\n" +
                    $"Pronadene javne metode: {imena}");
            }

            try
            {
                return metoda.Invoke(primjerak, argumenti);
            }
            catch (TargetInvocationException e) when (e.InnerException != null)
            {
                // Iznimku iz studentovog koda prosljedujemo neizmijenjenu,
                // inace bi svaki NotImplementedException izgledao kao kvar odraza.
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Postavlja javno svojstvo po imenu (zamjena strategije, stanja...).
        /// Prihvaca i javno polje istog imena - i jedno i drugo zadovoljava
        /// zahtjev "mora se moci zamijeniti u hodu".
        /// </summary>
        public static void Postavi(object primjerak, string nazivSvojstva, object vrijednost)
        {
            Type tip = primjerak.GetType();

            PropertyInfo svojstvo = tip.GetProperty(nazivSvojstva, BindingFlags.Instance | BindingFlags.Public);
            if (svojstvo != null && svojstvo.CanWrite)
            {
                svojstvo.SetValue(primjerak, vrijednost);
                return;
            }

            FieldInfo polje = tip.GetField(nazivSvojstva, BindingFlags.Instance | BindingFlags.Public);
            if (polje != null && !polje.IsInitOnly)
            {
                polje.SetValue(primjerak, vrijednost);
                return;
            }

            Assert.Fail(
                $"Razred '{tip.Name}' nema javno svojstvo ni polje '{nazivSvojstva}' koje se moze postaviti.\n" +
                "Zadatak trazi da se moze zamijeniti u hodu, npr. 'public ITaxStrategy Strategy { get; set; }'.");
        }

        private static string PotpisKonstruktora(ConstructorInfo k)
            => $"{k.DeclaringType.Name}({string.Join(", ", k.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))})";

        private static string OpisArgumenata(object[] argumenti)
            => argumenti.Length == 0
                ? "bez parametara"
                : string.Join(", ", argumenti.Select(a => a == null ? "null" : a.GetType().Name));
    }
}
