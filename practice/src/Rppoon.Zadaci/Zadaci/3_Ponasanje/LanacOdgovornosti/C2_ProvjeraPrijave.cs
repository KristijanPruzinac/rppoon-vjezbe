// ==================================================================
//  LANAC ODGOVORNOSTI - razina C (ispitna), zadatak 2: Provjera prijave
// ==================================================================
//  Ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Radite na obrascu za registraciju korisnika. Podatke treba provjeriti
//  nizom medusobno neovisnih pravila. Korisniku se moraju prikazati SVE
//  pogreske odjednom, a ne samo prva. Pravila se moraju moci dodavati,
//  micati i preslagivati bez izmjene postojecih razreda, a klijentski kod
//  ne smije znati koja se pravila primjenjuju.
//
//  prostor imena:  Rppoon.Zadaci.LanacOdgovornosti.C2
//
//  Registration        Registration(string username, string password, int age)
//                      string Username { get; }
//                      string Password { get; }
//                      int Age { get; }
//
//  ValidationHandler   apstraktni; DRZI referencu na sljedeceg obradivaca
//                      void SetNext(ValidationHandler handler)
//                      IList<string> Validate(Registration registration)
//                          vraca SVE pogreske - svoju (ako je ima) pa
//                          one koje javi ostatak lanca, tim redom
//
//  UsernameValidator   Username je null, prazan ili kraci od 3 znaka
//                          -> pogreska "korisnicko ime"
//  PasswordValidator   Password je null ili kraci od 8 znakova
//                          -> pogreska "lozinka"
//  AgeValidator        Age < 18
//                          -> pogreska "dob"
//
//  PRAVILA
//   - NECISTI lanac: pravilo odradi svoje pa SVEJEDNO proslijedi dalje.
//     Usporedi s C1, gdje se lanac prekida na prvom obradivacu.
//   - Ako je sve u redu, vraca se prazan popis (ne null).
//   - Nijedno pravilo ne smije imati polje ni svojstvo tipa nekog drugog
//     KONKRETNOG pravila - vidi samo ValidationHandler.
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.LanacOdgovornosti.C2
{
    // Pisi ovdje.
}
