using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.B2
{
    // ===== ALTERNATIVNO RJESENJE - Memento B2 =====
    //
    // Drugi od dva nacina koje zadatak spominje: snimka nije ugnijezdena
    // u uredivac nego je zaseban razred oznacen s 'internal'. Vidljiva
    // je unutar ovog sklopa, ali ne i izvana - a testovi su izvana.
    //
    // Ostalo je takoder napisano drugom rukom: povijest je lista, a
    // rastumacivanje snimke ide uzorkom 'is ... snapshot'.

    public interface IEditorMemento
    {
    }

    /// <summary>Konkretna snimka - izvan sklopa je nema.</summary>
    internal class EditorSnapshot : IEditorMemento
    {
        internal EditorSnapshot(int brightness, int contrast, string filter)
        {
            this.Brightness = brightness;
            this.Contrast = contrast;
            this.Filter = filter;
        }

        internal int Brightness { get; }

        internal int Contrast { get; }

        internal string Filter { get; }
    }

    public class ImageEditor
    {
        private int brightness;
        private int contrast;
        private string filter = "bez";

        public int Brightness => this.brightness;

        public int Contrast => this.contrast;

        public string Filter => this.filter;

        public void Adjust(int brightness, int contrast, string filter)
        {
            this.brightness = brightness;
            this.contrast = contrast;
            this.filter = filter;
        }

        public IEditorMemento Save()
        {
            return new EditorSnapshot(this.brightness, this.contrast, this.filter);
        }

        public void Restore(IEditorMemento memento)
        {
            if (memento is EditorSnapshot snapshot)
            {
                this.brightness = snapshot.Brightness;
                this.contrast = snapshot.Contrast;
                this.filter = snapshot.Filter;
            }
        }
    }

    public class EditHistory
    {
        private readonly List<IEditorMemento> mementos = new List<IEditorMemento>();

        public int Count => this.mementos.Count;

        public void Push(IEditorMemento memento)
        {
            this.mementos.Add(memento);
        }

        public IEditorMemento Pop()
        {
            if (this.mementos.Count == 0)
            {
                return null;
            }

            int zadnji = this.mementos.Count - 1;
            IEditorMemento memento = this.mementos[zadnji];
            this.mementos.RemoveAt(zadnji);
            return memento;
        }
    }

    public class PhotoEditorApp
    {
        private readonly ImageEditor editor = new ImageEditor();
        private readonly EditHistory history = new EditHistory();

        public int Brightness => this.editor.Brightness;

        public int Contrast => this.editor.Contrast;

        public string Filter => this.editor.Filter;

        public void Adjust(int brightness, int contrast, string filter)
        {
            this.history.Push(this.editor.Save());
            this.editor.Adjust(brightness, contrast, filter);
        }

        public bool Undo()
        {
            if (this.history.Count == 0)
            {
                return false;
            }

            this.editor.Restore(this.history.Pop());
            return true;
        }
    }
}
