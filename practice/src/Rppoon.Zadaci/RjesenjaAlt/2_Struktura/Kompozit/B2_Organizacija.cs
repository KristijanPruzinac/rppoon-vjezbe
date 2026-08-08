using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.B2
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit B2 =====
    //
    // Sve tri operacije preko LINQ-a: Sum za placu i broj ljudi,
    // DefaultIfEmpty + Max za dubinu (prazan popis daje 0).

    public interface IEmployee
    {
        string Name { get; }
        decimal TotalSalary();
        int HeadCount();
        int Depth();
    }

    public class Developer : IEmployee
    {
        private readonly decimal salary;

        public Developer(string name, decimal salary)
        {
            this.Name = name;
            this.salary = salary;
        }

        public string Name { get; private set; }

        public decimal TotalSalary() => this.salary;

        public int HeadCount() => 1;

        public int Depth() => 1;
    }

    public class Manager : IEmployee
    {
        private readonly decimal salary;
        private readonly List<IEmployee> podredeni = new List<IEmployee>();

        public Manager(string name, decimal salary)
        {
            this.Name = name;
            this.salary = salary;
        }

        public string Name { get; private set; }

        public void Add(IEmployee employee) => this.podredeni.Add(employee);

        public void Remove(IEmployee employee) => this.podredeni.Remove(employee);

        public decimal TotalSalary() => this.salary + this.podredeni.Sum(p => p.TotalSalary());

        public int HeadCount() => 1 + this.podredeni.Sum(p => p.HeadCount());

        public int Depth() => 1 + this.podredeni.Select(p => p.Depth()).DefaultIfEmpty(0).Max();
    }
}
