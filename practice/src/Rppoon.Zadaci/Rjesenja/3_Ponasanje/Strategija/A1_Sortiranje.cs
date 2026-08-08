using System;
using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Strategija.A1
{
    // ============ REFERENTNO RJESENJE - Strategija A1 ============

    public interface ISortStrategy
    {
        void Sort(List<string> list);
    }

    public class AscendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort();
        }
    }

    public class DescendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            list.Sort();
            list.Reverse();
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
