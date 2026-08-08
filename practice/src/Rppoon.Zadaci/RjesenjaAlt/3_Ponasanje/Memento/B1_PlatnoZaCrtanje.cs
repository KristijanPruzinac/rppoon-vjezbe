using System.Collections.Generic;

namespace Rppoon.Zadaci.Memento.B1
{
    // ===== ALTERNATIVNO RJESENJE - Memento B1 =====
    //
    // Druga ruka na sva tri mjesta:
    //   - snimka pamti POLJE (Shape[]) i vraca ga metodom, ne svojstvom
    //   - povijest je obicna lista; kad prijede deset, mice se element 0
    //   - popis vrsta se gradi petljom umjesto LINQ-om
    //
    // Ono sto se NIJE promijenilo je sam obrazac: snimku i dalje pravi i
    // cita samo platno, a povijest je i dalje netko treci.

    public class Shape
    {
        public Shape(string kind)
        {
            this.Kind = kind;
        }

        public string Kind { get; private set; }
    }

    public class ScreenState
    {
        private readonly Shape[] snimljeni;

        internal ScreenState(List<Shape> shapes)
        {
            this.snimljeni = shapes.ToArray();
        }

        internal Shape[] Snimljeni()
        {
            return this.snimljeni;
        }
    }

    public class DrawingScreen
    {
        private readonly List<Shape> shapes = new List<Shape>();

        public IReadOnlyList<string> Shapes
        {
            get
            {
                List<string> vrste = new List<string>();
                foreach (Shape shape in this.shapes)
                {
                    vrste.Add(shape.Kind);
                }
                return vrste;
            }
        }

        public void Draw(Shape shape)
        {
            this.shapes.Add(shape);
        }

        public ScreenState Save()
        {
            return new ScreenState(this.shapes);
        }

        public void Restore(ScreenState state)
        {
            this.shapes.Clear();
            this.shapes.AddRange(state.Snimljeni());
        }
    }

    public class DrawingHistory
    {
        public const int Kapacitet = 10;

        private readonly List<ScreenState> states = new List<ScreenState>();

        public int Count => this.states.Count;

        public void Push(ScreenState state)
        {
            if (this.states.Count == Kapacitet)
            {
                this.states.RemoveAt(0);
            }

            this.states.Add(state);
        }

        public ScreenState Pop()
        {
            if (this.states.Count == 0)
            {
                return null;
            }

            int zadnji = this.states.Count - 1;
            ScreenState state = this.states[zadnji];
            this.states.RemoveAt(zadnji);
            return state;
        }
    }

    public class DrawingApp
    {
        private readonly DrawingScreen screen = new DrawingScreen();
        private readonly DrawingHistory history = new DrawingHistory();

        public IReadOnlyList<string> Shapes => this.screen.Shapes;

        public void Add(string kind)
        {
            this.history.Push(this.screen.Save());
            this.screen.Draw(new Shape(kind));
        }

        public bool Undo()
        {
            if (this.history.Count == 0)
            {
                return false;
            }

            this.screen.Restore(this.history.Pop());
            return true;
        }
    }
}
