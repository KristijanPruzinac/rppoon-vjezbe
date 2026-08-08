// ==================================================================
//  STRATEGIJA - razina C (ispitna), zadatak 1: Obracun poreza
// ==================================================================
//  Ovdje NEMA kostura. Sve pises sam, kao na pismenom ispitu.
//  Testovi te pronalaze preko odraza (reflection), pa nazivi moraju
//  biti tocno ovakvi:
//
//  prostor imena:  Rppoon.Zadaci.Strategija.C1
//
//  ITaxStrategy          sucelje, metoda:  decimal Calculate(decimal amount)
//  StandardTax           konkretna strategija, 25 %
//  ReducedTax            konkretna strategija, 13 %
//  ZeroTax               konkretna strategija,  0 %
//
//  Invoice               kontekst
//      Invoice(decimal amount, ITaxStrategy strategy)
//      ITaxStrategy Strategy { get; set; }     // zamjena u hodu
//      decimal Total()                          // iznos + porez
//
//  Uvjet zadatka: Invoice NE SMIJE sadrzavati grananje po vrsti
//  poreza (if / switch). Odabir stope je odgovornost strategije.
//
//  Svi tipovi moraju biti 'public' da bi ih testovi vidjeli.
// ==================================================================

namespace Rppoon.Zadaci.Strategija.C1
{
    // Pisi ovdje.
}
