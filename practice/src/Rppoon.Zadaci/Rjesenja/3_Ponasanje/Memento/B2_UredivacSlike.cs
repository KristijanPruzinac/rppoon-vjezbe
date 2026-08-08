using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.B2
{
    // ===== REFERENTNO RJESENJE - Memento B2 =====
    //
    // Snimka je PRIVATNI ugnijezdeni razred uredivaca. Izvan uredivaca
    // se o njoj zna samo da je IEditorMemento, a to sucelje ne obecava
    // nista - pa se iz njega nista i ne moze izvuci.
    //
    // Uredivac zna pravi tip, pa ga u Restore rastumaci natrag.

    public interface IEditorMemento
    {
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
            return new Postavke(this.brightness, this.contrast, this.filter);
        }

        public void Restore(IEditorMemento memento)
        {
            Postavke snimka = memento as Postavke;
            if (snimka == null)
            {
                return;
            }

            this.brightness = snimka.Brightness;
            this.contrast = snimka.Contrast;
            this.filter = snimka.Filter;
        }

        /// <summary>Konkretna snimka - vidi je samo uredivac.</summary>
        private class Postavke : IEditorMemento
        {
            public Postavke(int brightness, int contrast, string filter)
            {
                this.Brightness = brightness;
                this.Contrast = contrast;
                this.Filter = filter;
            }

            public int Brightness { get; private set; }

            public int Contrast { get; private set; }

            public string Filter { get; private set; }
        }
    }

    public class EditHistory
    {
        private readonly Stack<IEditorMemento> mementos = new Stack<IEditorMemento>();

        public int Count => this.mementos.Count;

        public void Push(IEditorMemento memento)
        {
            this.mementos.Push(memento);
        }

        public IEditorMemento Pop()
        {
            return this.mementos.Count > 0 ? this.mementos.Pop() : null;
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
            IEditorMemento prethodno = this.history.Pop();
            if (prethodno == null)
            {
                return false;
            }

            this.editor.Restore(prethodno);
            return true;
        }
    }
}
