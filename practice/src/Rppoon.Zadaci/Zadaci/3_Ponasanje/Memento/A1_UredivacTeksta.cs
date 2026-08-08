using System;
using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Memento.A1
{
    // ================================================================
    //  MEMENTO - razina A (vodeno), zadatak 1: Uredivac teksta
    // ================================================================
    //  Zadatak je preuzet s ispita (5.9.2022., pitanje 8).
    //
    //  Tri uloge:
    //    TVORAC (originator)  TextEditor      - drzi stanje
    //    MEMENTO              TextSnapshot    - snimka stanja
    //    SKRBNIK (caretaker)  SnapshotStorage - cuva snimke,
    //                                           ali ih NE otvara
    //
    //  Sve osim tvorca je vec napisano. Ti pises dvije metode kojima
    //  tvorac snima i vraca vlastito stanje.
    //
    //  Bit obrasca: snimku stvara i cita ISKLJUCIVO tvorac. Skrbnik je
    //  drzi u rukama kao zatvorenu kutiju - zato zna kada vratiti
    //  stanje, ali ne zna sto je u njemu.
    // ================================================================

    /// <summary>DANO: memento - snimka stanja uredivaca.</summary>
    public class TextSnapshot
    {
        public TextSnapshot(string text)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }

    /// <summary>
    /// DANO: skrbnik. Gomila snimke i vraca ih obrnutim redom.
    /// Nigdje ne dira TextSnapshot.Text - to nije njegov posao.
    /// </summary>
    public class SnapshotStorage
    {
        private readonly Stack<TextSnapshot> snapshots = new Stack<TextSnapshot>();

        public int Count => this.snapshots.Count;

        public void Add(TextSnapshot textSnapshot)
        {
            this.snapshots.Push(textSnapshot);
        }

        /// <summary>Skida i vraca zadnju snimku; null ako ih vise nema.</summary>
        public TextSnapshot GetLast()
        {
            return this.snapshots.Count > 0 ? this.snapshots.Pop() : null;
        }
    }

    /// <summary>TVORAC: drzi tekst i zna ga snimiti i vratiti.</summary>
    public class TextEditor
    {
        private StringBuilder stringBuilder = new StringBuilder();

        public void Add(string text)
        {
            this.stringBuilder.Append(text);
        }

        public string GetText()
        {
            return this.stringBuilder.ToString();
        }

        /// <summary>Stvara snimku trenutnog stanja.</summary>
        public TextSnapshot Store()
        {
            // TODO
            //  Vrati NOVU snimku s trenutnim tekstom.
            //  Pazi: snimka mora biti neovisna o uredivacu. Kad poslije
            //  dopises jos teksta, vec stvorena snimka se ne smije
            //  promijeniti.
            throw new NotImplementedException("TextEditor.Store");
        }

        /// <summary>Vraca uredivac u stanje zapisano u snimci.</summary>
        public void Restore(TextSnapshot textSnapshot)
        {
            // TODO
            //  Postavi stanje uredivaca na tekst iz snimke.
            //  Pazi: nakon vracanja se mora moci normalno nastaviti
            //  pisati - Add() se nadovezuje na vraceni tekst.
            throw new NotImplementedException("TextEditor.Restore");
        }
    }

    /// <summary>
    /// DANO: klijent. Spaja tvorca i skrbnika. Primijeti da vracanje
    /// stanja pokrece klijent, a ne sam uredivac.
    /// </summary>
    public class TextWindow
    {
        private readonly TextEditor textEditor;
        private readonly SnapshotStorage snapshotStorage;

        public TextWindow(TextEditor textEditor)
        {
            this.textEditor = textEditor;
            this.snapshotStorage = new SnapshotStorage();
        }

        public string Text => this.textEditor.GetText();

        public void OnTextEntered(string text)
        {
            this.textEditor.Add(text);
        }

        public void OnSavePressed()
        {
            this.snapshotStorage.Add(this.textEditor.Store());
        }

        public void OnUndoPressed()
        {
            TextSnapshot snapshot = this.snapshotStorage.GetLast();
            if (snapshot != null)
            {
                this.textEditor.Restore(snapshot);
            }
        }
    }
}
