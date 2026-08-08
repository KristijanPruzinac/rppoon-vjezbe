// ==================================================================
//  KOMPOZIT - razina C (ispitna), zadatak 1: Oblici na zaslonu
// ==================================================================
//  Ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Radite na alatu namijenjenom predskolskoj djeci koji treba
//  omoguciti prikaz razlicitih oblika na ekranu. Postoji vise
//  konkretnih oblika (kruznica, pravokutnik), a uz njih i slozeni
//  oblici koji se sastoje od vise osnovnih ili cak vise drugih
//  slozenih oblika. Bez obzira o kakvom je obliku rijec, klijentski
//  kod ocekuje funkcionalnost iscrtavanja i funkcionalnost
//  odredivanja krajnje lijeve, desne i gornje tocke oblika.
//
//  prostor imena:  Rppoon.Zadaci.Kompozit.C1
//
//  IShape          string Draw()     opis iscrtavanja
//                  double Left()     krajnja lijeva tocka (x)
//                  double Right()    krajnja desna tocka (x)
//                  double Top()      krajnja gornja tocka (y)
//
//  Circle          Circle(double centerX, double centerY, double radius)
//                  Draw() vraca "krug"
//                  Left = centerX - radius, Right = centerX + radius
//                  Top  = centerY + radius        (y raste prema gore)
//
//  Rectangle       Rectangle(double x, double y, double width, double height)
//                  (x, y) je DONJI LIJEVI kut
//                  Draw() vraca "pravokutnik"
//                  Left = x, Right = x + width, Top = y + height
//
//  CompositeShape  CompositeShape()
//                  void Add(IShape shape) / void Remove(IShape shape)
//                  Draw() vraca opise djece spojene znakom "+"
//                         npr. "krug+pravokutnik"; prazan slozeni
//                         oblik vraca prazan niz ""
//                  Left/Right/Top su krajnje tocke SVIH sadrzanih
//                         oblika; prazan slozeni oblik vraca 0
//
//  Uvjeti:
//    - slozeni oblik mora moci sadrzavati druge slozene oblike
//    - klijent ne smije morati razlikovati osnovni od slozenog oblika
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.Kompozit.C1
{
    // Pisi ovdje.
}
