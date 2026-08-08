// ==================================================================
//  SINGLETON - razina C (ispitna), zadatak 2: Veza na bazu
// ==================================================================
//  Nema kostura. Sve pises sam.
//
//  prostor imena:  Rppoon.Zadaci.Singleton.C2
//
//  DatabaseConnection   jedina veza na bazu u aplikaciji
//
//      static DatabaseConnection Instance { get; }
//      void Open()          otvara vezu; ako je vec otvorena, ne radi nista
//      void Close()         zatvara vezu; ako je vec zatvorena, ne radi nista
//      bool IsOpen          je li veza trenutno otvorena
//      int OpenCount        koliko je puta veza ukupno otvorena
//                           (ponovljeni Open na otvorenoj vezi se NE broji)
//
//  Uvjeti:
//    - 'new DatabaseConnection()' izvan razreda ne smije biti moguc
//    - dva poziva Instance daju istu referencu
//    - stanje je zajednicko: tko god dohvati Instance, vidi istu vezu
//
//  Tip mora biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.Singleton.C2
{
    // Pisi ovdje.
}
