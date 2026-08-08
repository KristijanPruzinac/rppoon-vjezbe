using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Memento.B1
{
    // ===== REFERENTNO RJESENJE - Memento B1 =====
    //
    // Snimka kopira listu u konstruktoru. Zato ScreenState nema nijedan
    // javni clan: ono sto je unutra treba samo platnu, a ono ga uzima
    // preko svojstva koje nije javno (internal).
    //
    // Ogranicenje na deset je rijeseno LinkedList-om: dodaje se na kraj,
    // a kad prijede kapacitet, mice se prvi (najstariji) clan. Stog to
    // ne bi mogao bez prepisivanja cijele zbirke.

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
        private readonly List<Shape> shapes;

        internal ScreenState(IEnumerable<Shape> shapes)
        {
            this.shapes = new List<Shape>(shapes);
        }

        internal IReadOnlyList<Shape> Shapes => this.shapes;
    }

    public class DrawingScreen
    {
        private readonly List<Shape> shapes = new List<Shape>();

        public IReadOnlyList<string> Shapes => this.shapes.Select(s => s.Kind).ToList();

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
            this.shapes.AddRange(state.Shapes);
        }
    }

    public class DrawingHistory
    {
        public const int Kapacitet = 10;

        private readonly LinkedList<ScreenState> states = new LinkedList<ScreenState>();

        public int Count => this.states.Count;

        public void Push(ScreenState state)
        {
            this.states.AddLast(state);

            if (this.states.Count > Kapacitet)
            {
                this.states.RemoveFirst();
            }
        }

        public ScreenState Pop()
        {
            if (this.states.Count == 0)
            {
                return null;
            }

            ScreenState zadnje = this.states.Last.Value;
            this.states.RemoveLast();
            return zadnje;
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
            ScreenState prethodno = this.history.Pop();
            if (prethodno == null)
            {
                return false;
            }

            this.screen.Restore(prethodno);
            return true;
        }
    }
}
