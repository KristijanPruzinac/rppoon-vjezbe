// ==================================================================
//  KOMPOZIT - razina C (ispitna), zadatak 2: Sastavnica proizvoda
// ==================================================================
//  Ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Radite na sustavu za planiranje proizvodnje. Proizvod se sastoji
//  od dijelova, a dio moze biti osnovni (kupuje se gotov) ili sklop
//  koji se i sam sastoji od drugih dijelova, proizvoljno duboko.
//  Za svaki dio treba znati ukupnu cijenu i ukupno vrijeme izrade.
//  Klijentski kod ne smije razlikovati osnovni dio od sklopa.
//
//  prostor imena:  Rppoon.Zadaci.Kompozit.C2
//
//  IPart            string Name { get; }
//                   decimal Cost()        ukupna cijena
//                   int AssemblyMinutes() ukupno vrijeme izrade
//
//  BasicPart        BasicPart(string name, decimal cost)
//                   Cost() je vlastita cijena
//                   AssemblyMinutes() je 0 - osnovni dio se ne sastavlja
//
//  Assembly         Assembly(string name, int ownMinutes)
//                   void Add(IPart part, int quantity)
//                   void Remove(IPart part)
//                   Cost() = zbroj (cijena dijela x kolicina)
//                   AssemblyMinutes() = ownMinutes + zbroj
//                                       (vrijeme dijela x kolicina)
//
//  PAZI: kolicina se MNOZI kroz cijelo stablo. Sklop koji sadrzi dva
//  podsklopa, a svaki podsklop tri vijka, ukupno trosi sest vijaka.
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.Kompozit.C2
{
    // Pisi ovdje.
}
