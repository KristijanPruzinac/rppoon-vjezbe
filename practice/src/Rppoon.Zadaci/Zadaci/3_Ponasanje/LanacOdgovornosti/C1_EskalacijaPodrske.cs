// ==================================================================
//  LANAC ODGOVORNOSTI - razina C (ispitna), zadatak 1: Eskalacija
// ==================================================================
//  Ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Radite na sustavu za korisnicku podrsku. Prijava dolazi s ocjenom
//  ozbiljnosti, a rjesava je prva razina podrske koja je za nju
//  nadlezna. Ako nije, prijava se penje na sljedecu razinu. Razine se
//  moraju moci dodavati i preslagivati bez diranja postojecih razreda,
//  a posiljatelj prijave ne smije znati koja ce je razina rijesiti.
//
//  prostor imena:  Rppoon.Zadaci.LanacOdgovornosti.C1
//
//  SupportTicket    SupportTicket(string subject, int severity)
//                   string Subject { get; }
//                   int Severity { get; }
//                   IList<string> Trail { get; }   pocetno prazan
//
//  SupportHandler   apstraktni; DRZI referencu na sljedeceg obradivaca
//                   void SetNext(SupportHandler handler)
//                   string Handle(SupportTicket ticket)
//                       vraca naziv razreda koji je prijavu rijesio,
//                       ili null ako je nije rijesio nitko u lancu
//
//  Level1Support    rjesava Severity <= 1
//  Level2Support    rjesava Severity <= 3
//  EngineeringTeam  rjesava sve
//
//  PRAVILA
//   - Svaki obradivac na POCETKU svog Handle upise naziv svog razreda
//     u ticket.Trail (npr. "Level1Support"). Tako se vidi dokle je
//     prijava putovala.
//   - CISTI lanac: cim netko rijesi, dalje se NE prosljeduje.
//   - Nijedan konkretan obradivac ne smije imati polje ni svojstvo tipa
//     nekog drugog KONKRETNOG obradivaca - vidi samo SupportHandler.
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.LanacOdgovornosti.C1
{
    // Pisi ovdje.
}
