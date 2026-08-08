using System;
using System.Collections.Generic;
using System.Text;

namespace Rppoon.Zadaci.Strategija.A1
{
    // ============================================================
    //  STRATEGIJA - razina A (vodeno), zadatak 1: Sortiranje imena
    // ============================================================
    //  Cijela gradja obrasca vec postoji. Tvoj posao su samo dva
    //  tijela metode oznacena s TODO.
    //
    //  Pitanje na koje zadatak odgovara: zasto kontekst (SortedList)
    //  ne zna NIKAD kako se sortira, nego samo koga pitati?
    // ============================================================

    /// <summary>Apstraktna strategija - ugovor koji sve strategije dijele.</summary>
    public interface ISortStrategy
    {
        void Sort(List<string> list);
    }

    /// <summary>Konkretna strategija: rastuci poredak.</summary>
    public class AscendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            // TODO: sortiraj listu rastuce (abecedno).
            throw new NotImplementedException("AscendingSort.Sort");
        }
    }

    /// <summary>Konkretna strategija: padajuci poredak.</summary>
    public class DescendingSort : ISortStrategy
    {
        public void Sort(List<string> list)
        {
            // TODO: sortiraj listu padajuce.
            throw new NotImplementedException("DescendingSort.Sort");
        }
    }

    /// <summary>
    /// Kontekst. DAN JE U CIJELOSTI - promotri da nigdje ne stoji
    /// "if (poredak == ...)": kontekst samo delegira strategiji.
    /// </summary>
    public class SortedList
    {
        private List<string> Students { get; set; }

        public ISortStrategy Strategy { get; set; }

        public SortedList(List<string> students, ISortStrategy strategy)
        {
            this.Students = students;
            this.Strategy = strategy;
        }

        /// <summary>Trenutni sadrzaj, radi provjere u testovima.</summary>
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
