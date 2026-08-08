using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.B1
{
    // ================================================================
    //  MEMENTO - razina B (sastavi), zadatak 1: Platno za crtanje
    // ================================================================
    //  Zadatak s ispita (26.6.2023., pitanje 10), doslovno:
    //
    //  > Radite na didaktickom alatu za djecu koji omogucuje iscrtavanje
    //  > razlicitih oblika na zaslon. Unutar klase koja predstavlja
    //  > zaslon cuva se kolekcija objekata koji predstavljaju oblike
    //  > koji NISU javno dostupni. Trebate omoguciti cuvanje trenutnog
    //  > stanja zaslona i funkcionalnost undo tako da dijete moze
    //  > ponistiti posljednju radnju (zadnji dodani oblik) i vratiti
    //  > crtez na neko prethodno stanje. Stanje se biljezi svakim
    //  > dodavanjem novog lika na zaslon i mora se omoguciti cuvanje
    //  > ZADNJIH DESET stanja. Obavezno morate ocuvati enkapsulaciju.
    //
    //  Dobio si potpise. Tijela su tvoja.
    //
    //  Dvije zamke u kojima ovaj zadatak najcesce padne:
    //
    //   1. SNIMKA KOJA NIJE SNIMKA. Stanje zaslona je LISTA. Ako snimka
    //      zapamti istu listu koju zaslon i dalje mijenja, snimka se
    //      mijenja zajedno s njim - i vracanje ne vraca nista. Snimka
    //      mora imati SVOJU kopiju.
    //
    //   2. OGRANICENJE OD DESET. Kad dode jedanaesto stanje, ispada
    //      NAJSTARIJE, a ne najnovije. Stog to sam od sebe ne radi.
    // ================================================================

    /// <summary>DANO: oblik na platnu.</summary>
    public class Shape
    {
        public Shape(string kind)
        {
            this.Kind = kind;
        }

        public string Kind { get; private set; }
    }

    /// <summary>
    /// MEMENTO: snimka stanja platna.
    /// Sto tocno pamti i kako - tvoja odluka. Izvana se ne smije
    /// mijenjati.
    /// </summary>
    public class ScreenState
    {
        // TODO
    }

    /// <summary>
    /// TVORAC: platno. Zbirka oblika je privatna - klijent do nje ne
    /// smije doci u obliku koji moze mijenjati.
    /// </summary>
    public class DrawingScreen
    {
        // TODO: privatna zbirka oblika

        /// <summary>Vrste oblika na platnu, redom kojim su nacrtani.</summary>
        public IReadOnlyList<string> Shapes
        {
            get { throw new NotImplementedException("DrawingScreen.Shapes"); }
        }

        /// <summary>Dodaje oblik na platno.</summary>
        public void Draw(Shape shape)
        {
            throw new NotImplementedException("DrawingScreen.Draw");
        }

        /// <summary>Stvara snimku trenutnog stanja platna.</summary>
        public ScreenState Save()
        {
            throw new NotImplementedException("DrawingScreen.Save");
        }

        /// <summary>Vraca platno u stanje zapisano u snimci.</summary>
        public void Restore(ScreenState state)
        {
            throw new NotImplementedException("DrawingScreen.Restore");
        }
    }

    /// <summary>
    /// SKRBNIK: povijest crteza. Cuva NAJVISE deset stanja; kad dode
    /// jedanaesto, najstarije ispada.
    /// </summary>
    public class DrawingHistory
    {
        /// <summary>Najveci broj stanja koja se cuvaju.</summary>
        public const int Kapacitet = 10;

        // TODO: zbirka snimki

        /// <summary>Koliko je snimki trenutno spremljeno.</summary>
        public int Count
        {
            get { throw new NotImplementedException("DrawingHistory.Count"); }
        }

        /// <summary>Sprema snimku; ako ih je vec deset, najstarija ispada.</summary>
        public void Push(ScreenState state)
        {
            throw new NotImplementedException("DrawingHistory.Push");
        }

        /// <summary>Skida i vraca zadnju snimku; null ako je prazno.</summary>
        public ScreenState Pop()
        {
            throw new NotImplementedException("DrawingHistory.Pop");
        }
    }

    /// <summary>KLIJENT: alat kojim se dijete sluzi.</summary>
    public class DrawingApp
    {
        // TODO: platno i povijest

        /// <summary>Vrste oblika trenutno na platnu.</summary>
        public IReadOnlyList<string> Shapes
        {
            get { throw new NotImplementedException("DrawingApp.Shapes"); }
        }

        /// <summary>
        /// Crta novi oblik. Prije crtanja se biljezi stanje kakvo je
        /// bilo - to je stanje na koje ce undo vratiti.
        /// </summary>
        public void Add(string kind)
        {
            throw new NotImplementedException("DrawingApp.Add");
        }

        /// <summary>
        /// Ponistava zadnju radnju. Vraca true ako je bilo sto
        /// ponistiti, inace false.
        /// </summary>
        public bool Undo()
        {
            throw new NotImplementedException("DrawingApp.Undo");
        }
    }
}
