using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.C2
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit C2 =====
    //
    // Kolicine u popisu n-torki umjesto u ugnijezdenom razredu,
    // zbrajanje LINQ-om, ime u izricitom polju.

    public interface IPart
    {
        string Name { get; }
        decimal Cost();
        int AssemblyMinutes();
    }

    public class BasicPart : IPart
    {
        private readonly string name;
        private readonly decimal cost;

        public BasicPart(string name, decimal cost)
        {
            this.name = name;
            this.cost = cost;
        }

        public string Name => this.name;

        public decimal Cost() => this.cost;

        public int AssemblyMinutes() => 0;
    }

    public class Assembly : IPart
    {
        private readonly string name;
        private readonly int ownMinutes;
        private readonly List<(IPart Dio, int Kolicina)> sadrzaj = new List<(IPart, int)>();

        public Assembly(string name, int ownMinutes)
        {
            this.name = name;
            this.ownMinutes = ownMinutes;
        }

        public string Name => this.name;

        public void Add(IPart part, int quantity)
        {
            this.sadrzaj.Add((part, quantity));
        }

        public void Remove(IPart part)
        {
            this.sadrzaj.RemoveAll(s => ReferenceEquals(s.Dio, part));
        }

        public decimal Cost()
        {
            return this.sadrzaj.Sum(s => s.Dio.Cost() * s.Kolicina);
        }

        public int AssemblyMinutes()
        {
            return this.ownMinutes + this.sadrzaj.Sum(s => s.Dio.AssemblyMinutes() * s.Kolicina);
        }
    }
}
