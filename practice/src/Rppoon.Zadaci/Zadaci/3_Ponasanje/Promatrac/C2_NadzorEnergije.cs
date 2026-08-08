// ==================================================================
//  PROMATRAC - razina C (ispitna), zadatak 2: Nadzor energije
// ==================================================================
//  Ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Radite na sustavu za nadzor proizvodnje i potrosnje elektricne
//  energije. Primarna uloga je pracenje stanja ukupne proizvodnje i
//  ukupne potrosnje. Vise razlicitih, raznorodnih komponenti mora se
//  sinkronizirati s tim stanjima i biti automatski obavijesteno o
//  promjeni, a taj se odnos mora moci ostvariti i prekinuti dinamicki.
//
//  Komponenta za biljezenje (logging) treba zabiljeziti vrijednosti
//  pri svakoj izmjeni. Komponenta za rezervu (backup) treba, u slucaju
//  da potrosnja dosegne 90 % ili vise iznosa proizvodnje, aktivirati
//  dodatne kapacitete. U buducnosti ce trebati dodati nove komponente
//  bez utjecaja na klijentski kod.
//
//  prostor imena:  Rppoon.Zadaci.Promatrac.C2
//
//  IEnergyObserver    void OnEnergyChanged(double production, double consumption)
//
//  EnergyGrid         void SetProduction(double value)
//                     void SetConsumption(double value)
//                     double Production { get; }
//                     double Consumption { get; }
//                     void Attach(IEnergyObserver observer)
//                     void Detach(IEnergyObserver observer)
//                     Svaka izmjena proizvodnje ILI potrosnje salje obavijest.
//
//  EnergyLogger       int Entries { get; }          broj zabiljezenih izmjena
//                     double LastProduction { get; }
//                     double LastConsumption { get; }
//
//  BackupUnit         bool Activated { get; }       true cim potrosnja
//                                                   dosegne 90 % proizvodnje
//                     int Activations { get; }      koliko je puta aktiviran
//                     Ako je proizvodnja 0, rezerva se NE aktivira
//                     (nema se sto usporediti - izbjegni dijeljenje nulom).
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.Promatrac.C2
{
    // Pisi ovdje.
}
