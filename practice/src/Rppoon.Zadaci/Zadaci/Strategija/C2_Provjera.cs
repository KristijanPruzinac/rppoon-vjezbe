// ==================================================================
//  STRATEGIJA - razina C (ispitna), zadatak 2: Provjera unosa
// ==================================================================
//  Nema kostura. Sve pises sam.
//
//  prostor imena:  Rppoon.Zadaci.Strategija.C2
//
//  IValidationStrategy      sucelje s TOCNO JEDNOM metodom:
//                               bool IsValid(string input)
//
//  NotEmptyValidation       unos nije null ni sam od praznina
//  MinLengthValidation      konstruktor prima int minLength;
//                               unos mora imati barem toliko znakova
//  DigitRequiredValidation  unos mora sadrzavati barem jednu znamenku
//
//  RegistrationForm         kontekst
//      RegistrationForm(IValidationStrategy strategy)
//      IValidationStrategy Strategy { get; set; }   // zamjena u hodu
//      bool Check(string input)
//
//  Uvjet: RegistrationForm ne smije znati NIJEDNO pravilo provjere.
//  On samo pita strategiju.
//
//  Svi tipovi moraju biti 'public'. Nazivi moraju biti tocno ovakvi
//  jer te testovi traze odrazom - unutarnju izvedbu biras sam.
// ==================================================================

namespace Rppoon.Zadaci.Strategija.C2
{
    // Pisi ovdje.
}
