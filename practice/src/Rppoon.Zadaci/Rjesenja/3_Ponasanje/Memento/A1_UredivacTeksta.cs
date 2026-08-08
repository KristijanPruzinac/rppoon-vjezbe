using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Memento.A1
{
    // ===== REFERENTNO RJESENJE - Memento A1 =====
    //
    // Store() vadi niz znakova iz graditelja. Niz je nepromjenjiv, pa je
    // snimka time automatski odvojena od uredivaca.
    //
    // Restore() gradi NOVI StringBuilder iz snimljenog teksta. Da smo
    // samo zapamtili niz uz stari graditelj, Add() bi se nastavio
    // nadovezivati na tekst od prije vracanja.

    public class TextSnapshot
    {
        public TextSnapshot(string text)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }

    public class SnapshotStorage
    {
        private readonly Stack<TextSnapshot> snapshots = new Stack<TextSnapshot>();

        public int Count => this.snapshots.Count;

        public void Add(TextSnapshot textSnapshot)
        {
            this.snapshots.Push(textSnapshot);
        }

        public TextSnapshot GetLast()
        {
            return this.snapshots.Count > 0 ? this.snapshots.Pop() : null;
        }
    }

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

        public TextSnapshot Store()
        {
            return new TextSnapshot(this.stringBuilder.ToString());
        }

        public void Restore(TextSnapshot textSnapshot)
        {
            this.stringBuilder = new StringBuilder(textSnapshot.Text);
        }
    }

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
