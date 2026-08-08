using System;

namespace Rppoon.Zadaci.Memento.B2
{
    // ================================================================
    //  MEMENTO - razina B (sastavi), zadatak 2: Uredivac slike
    // ================================================================
    //  Ispitni tekst zadatka o Mementu zavrsava recenicom "obavezno
    //  morate ocuvati enkapsulaciju". Ovaj zadatak je samo o njoj.
    //
    //  U zadatku A1 je snimka imala javno svojstvo Text. To je uredno
    //  za ispit, ali nije najstroze rjesenje: tko god drzi snimku, moze
    //  je i procitati. Ovdje to zatvaramo do kraja.
    //
    //  POSTUPAK (klasican, tzv. "usko grlo sucelja"):
    //
    //    - javno je samo PRAZNO sucelje IEditorMemento; ono nema
    //      nijednu metodu ni svojstvo
    //    - konkretna snimka je razred koji to sucelje implementira, ali
    //      NIJE javan - deklariras ga kao 'private' ugnijezden unutar
    //      uredivaca, ili kao 'internal' u ovom prostoru imena
    //    - skrbnik prima i vraca IEditorMemento; on tako nema sto ni
    //      procitati ni promijeniti
    //    - uredivac zna pravi tip pa ga pri vracanju moze rastumaciti
    //
    //  Rezultat: povijest se moze cuvati, prosljedivati i brojati, a da
    //  nitko osim uredivaca ne zna sto je u njoj.
    //
    //  Pises sve osim potpisa. Nijedan NOVI tip u ovom prostoru imena
    //  ne smije biti javan.
    // ================================================================

    /// <summary>
    /// MEMENTO, vanjski pogled: namjerno prazno. Nosi snimku, ali o njoj
    /// ne odaje nista.
    /// </summary>
    public interface IEditorMemento
    {
    }

    /// <summary>TVORAC: postavke obrade slike.</summary>
    public class ImageEditor
    {
        // TODO: stanje uredivaca (svjetlina, kontrast, filtar)
        //       i, ugnijezden ovdje, privatni razred snimke

        /// <summary>Svjetlina, pocetno 0.</summary>
        public int Brightness
        {
            get { throw new NotImplementedException("ImageEditor.Brightness"); }
        }

        /// <summary>Kontrast, pocetno 0.</summary>
        public int Contrast
        {
            get { throw new NotImplementedException("ImageEditor.Contrast"); }
        }

        /// <summary>Naziv filtra, pocetno "bez".</summary>
        public string Filter
        {
            get { throw new NotImplementedException("ImageEditor.Filter"); }
        }

        /// <summary>Postavlja sve tri vrijednosti odjednom.</summary>
        public void Adjust(int brightness, int contrast, string filter)
        {
            throw new NotImplementedException("ImageEditor.Adjust");
        }

        /// <summary>Stvara snimku trenutnih postavki.</summary>
        public IEditorMemento Save()
        {
            throw new NotImplementedException("ImageEditor.Save");
        }

        /// <summary>Vraca postavke iz snimke.</summary>
        public void Restore(IEditorMemento memento)
        {
            throw new NotImplementedException("ImageEditor.Restore");
        }
    }

    /// <summary>
    /// SKRBNIK: povijest izmjena. Primijeti da u cijelom razredu nema
    /// nijedne rijeci o svjetlini, kontrastu ni filtru - ne mora je ni
    /// biti.
    /// </summary>
    public class EditHistory
    {
        // TODO: zbirka snimki

        /// <summary>Koliko je snimki spremljeno.</summary>
        public int Count
        {
            get { throw new NotImplementedException("EditHistory.Count"); }
        }

        /// <summary>Sprema snimku.</summary>
        public void Push(IEditorMemento memento)
        {
            throw new NotImplementedException("EditHistory.Push");
        }

        /// <summary>Skida i vraca zadnju snimku; null ako je prazno.</summary>
        public IEditorMemento Pop()
        {
            throw new NotImplementedException("EditHistory.Pop");
        }
    }

    /// <summary>KLIJENT: spaja uredivac i povijest.</summary>
    public class PhotoEditorApp
    {
        // TODO: uredivac i povijest

        public int Brightness
        {
            get { throw new NotImplementedException("PhotoEditorApp.Brightness"); }
        }

        public int Contrast
        {
            get { throw new NotImplementedException("PhotoEditorApp.Contrast"); }
        }

        public string Filter
        {
            get { throw new NotImplementedException("PhotoEditorApp.Filter"); }
        }

        /// <summary>Zabiljezi stanje prije izmjene, pa izmijeni.</summary>
        public void Adjust(int brightness, int contrast, string filter)
        {
            throw new NotImplementedException("PhotoEditorApp.Adjust");
        }

        /// <summary>Ponistava zadnju izmjenu; false ako je nema.</summary>
        public bool Undo()
        {
            throw new NotImplementedException("PhotoEditorApp.Undo");
        }
    }
}
