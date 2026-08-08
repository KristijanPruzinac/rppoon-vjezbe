// ==================================================================
//  PROMATRAC - razina C (ispitna), zadatak 1: MooTube
// ==================================================================
//  Ovo je ispitni zadatak. Nema kostura - sve pises sam.
//
//  ZADATAK
//  Trebate napraviti sustav za objavu videozapisa naziva MooTube.
//  Svaki kreator sadrzaja (MooTuber) objavljuje videozapise. Potrebno
//  je omoguciti da korisnici prate pojedinog MooTubera i pri objavi
//  novog videozapisa dobiju obavijest s poveznicom. Istovremeno velik
//  broj korisnika mora moci pratiti objave. Pracenje mora biti
//  dinamicko: mora se moci zapoceti i prestati po zelji, bez utjecaja
//  na klijentski kod.
//
//  prostor imena:  Rppoon.Zadaci.Promatrac.C1
//
//  ISubscriber            void OnNewVideo(string channel, string url)
//
//  MooTuber               MooTuber(string channel)
//                         string Channel { get; }
//                         void Subscribe(ISubscriber subscriber)
//                         void Unsubscribe(ISubscriber subscriber)
//                         void Publish(string url)   objavi i obavijesti sve
//
//  Viewer                 Viewer()
//                         string LastUrl { get; }     zadnja primljena poveznica
//                         int NotificationCount { get; }
//
//  Uvjeti:
//    - MooTuber ne smije ovisiti o razredu Viewer, samo o ISubscriber
//    - isti Viewer mora se moci pretplatiti na vise MooTubera
//    - odjavljeni Viewer vise ne prima obavijesti
//
//  Svi tipovi moraju biti 'public'.
// ==================================================================

namespace Rppoon.Zadaci.Promatrac.C1
{
    // Pisi ovdje.
}
