using System;
using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Strategija.A1
{
    // ===== ALTERNATIVNO RJESENJE - Strategija A1 =====
    //
    // Ista dva zadatka rijesena drugim sredstvima: usporedbenim
    // delegatom umjesto Sort() + Reverse(). Rezultat je isti, put nije.

    public interface ISortStrategy
    {
        void Sort(List<string> list);
    }

    public class AscendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort(delegate (string lijevo, string desno)
            {
                return string.Compare(lijevo, desno, StringComparison.Ordinal);
            });
        }
    }

    public class DescendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort(delegate (string lijevo, string desno)
            {
                return string.Compare(desno, lijevo, StringComparison.Ordinal);
            });
        }
    }

    public class SortedList
    {
        private List<string> Students { get; set; }

        public ISortStrategy Strategy { get; set; }

        public SortedList(List<string> students, ISortStrategy strategy)
        {
            this.Students = students;
            this.Strategy = strategy;
        }

        public IReadOnlyList<string> Names
        {
            get { return this.Students; }
        }

        public void Add(string name)
        {
            this.Students.Add(name);
        }

        public void Sort()
        {
            this.Strategy.Sort(this.Students);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string name in this.Students)
            {
                builder.Append(name + Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}
