// ==================================================================
//  SINGLETON - razina C (ispitna), zadatak 1: Red za ispis
// ==================================================================
//  Nema kostura. Sve pises sam.
//
//  prostor imena:  Rppoon.Zadaci.Singleton.C1
//
//  PrintSpooler   jedini red za ispis u cijeloj aplikaciji
//
//      static PrintSpooler Instance { get; }   staticna tocka pristupa
//      void Send(string document)              dodaje dokument u red
//      int Pending                             koliko dokumenata ceka
//      string PrintNext()                      uzima najstariji dokument
//                                              i vraca ga; ako je red
//                                              prazan, vraca null
//
//  Uvjeti:
//    - 'new PrintSpooler()' izvan razreda ne smije biti moguc
//    - dva poziva Instance moraju dati istu referencu
//    - red se ponasa po nacelu prvi unutra - prvi van
//
//  Tip mora biti 'public'. Nacin stvaranja (lijeni, gladni, Lazy)
//  biras sam.
// ==================================================================

namespace Rppoon.Zadaci.Singleton.C1
{
    // Pisi ovdje.
}
