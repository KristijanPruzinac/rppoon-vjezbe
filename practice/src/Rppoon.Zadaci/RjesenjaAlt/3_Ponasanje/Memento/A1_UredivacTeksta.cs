using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Memento.A1
{
    // ===== ALTERNATIVNO RJESENJE - Memento A1 =====
    //
    // Isti obrazac, drugacija ruka:
    //   - Store() ide preko vec postojeceg GetText() umjesto da ponovo
    //     zove stringBuilder.ToString()
    //   - Restore() zadrzava isti graditelj i samo mu zamijeni sadrzaj
    //     (Clear + Append). Rezultat je isti kao da je stvoren novi.

    public class TextSnapshot
    {
        private readonly string text;

        public TextSnapshot(string text)
        {
            this.text = text;
        }

        public string Text => this.text;
    }

    public class SnapshotStorage
    {
        private readonly List<TextSnapshot> snapshots = new List<TextSnapshot>();

        public int Count => this.snapshots.Count;

        public void Add(TextSnapshot textSnapshot)
        {
            this.snapshots.Add(textSnapshot);
        }

        public TextSnapshot GetLast()
        {
            if (this.snapshots.Count == 0)
            {
                return null;
            }

            TextSnapshot zadnja = this.snapshots[this.snapshots.Count - 1];
            this.snapshots.RemoveAt(this.snapshots.Count - 1);
            return zadnja;
        }
    }

    public class TextEditor
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

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
            return new TextSnapshot(this.GetText());
        }

        public void Restore(TextSnapshot textSnapshot)
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append(textSnapshot.Text);
        }
    }

    public class TextWindow
    {
        private readonly TextEditor textEditor;
        private readonly SnapshotStorage snapshotStorage = new SnapshotStorage();

        public TextWindow(TextEditor textEditor)
        {
            this.textEditor = textEditor;
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
