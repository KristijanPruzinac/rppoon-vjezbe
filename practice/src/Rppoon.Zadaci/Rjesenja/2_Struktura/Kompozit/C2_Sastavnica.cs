using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.C2
{
    // ============ REFERENTNO RJESENJE - Kompozit C2 ============

    public interface IPart
    {
        string Name { get; }
        decimal Cost();
        int AssemblyMinutes();
    }

    public class BasicPart : IPart
    {
        private readonly decimal cost;

        public BasicPart(string name, decimal cost)
        {
            this.Name = name;
            this.cost = cost;
        }

        public string Name { get; private set; }

        public decimal Cost()
        {
            return this.cost;
        }

        public int AssemblyMinutes()
        {
            return 0;
        }
    }

    public class Assembly : IPart
    {
        private readonly int ownMinutes;
        private readonly List<Stavka> stavke = new List<Stavka>();

        public Assembly(string name, int ownMinutes)
        {
            this.Name = name;
            this.ownMinutes = ownMinutes;
        }

        public string Name { get; private set; }

        public void Add(IPart part, int quantity)
        {
            this.stavke.Add(new Stavka(part, quantity));
        }

        public void Remove(IPart part)
        {
            this.stavke.RemoveAll(s => s.Dio == part);
        }

        public decimal Cost()
        {
            decimal ukupno = 0m;
            foreach (Stavka stavka in this.stavke)
            {
                ukupno += stavka.Dio.Cost() * stavka.Kolicina;
            }
            return ukupno;
        }

        public int AssemblyMinutes()
        {
            int ukupno = this.ownMinutes;
            foreach (Stavka stavka in this.stavke)
            {
                ukupno += stavka.Dio.AssemblyMinutes() * stavka.Kolicina;
            }
            return ukupno;
        }

        /// <summary>Par dijela i kolicine - kolicina pripada VEZI, ne dijelu.</summary>
        private class Stavka
        {
            public Stavka(IPart dio, int kolicina)
            {
                this.Dio = dio;
                this.Kolicina = kolicina;
            }

            public IPart Dio { get; private set; }

            public int Kolicina { get; private set; }
        }
    }
}
