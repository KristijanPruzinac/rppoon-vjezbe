using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.B2
{
    // ============ REFERENTNO RJESENJE - Kompozit B2 ============

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

        public decimal TotalSalary()
        {
            return this.salary;
        }

        public int HeadCount()
        {
            return 1;
        }

        public int Depth()
        {
            return 1;
        }
    }

    public class Manager : IEmployee
    {
        private readonly decimal salary;
        private readonly List<IEmployee> subordinates = new List<IEmployee>();

        public Manager(string name, decimal salary)
        {
            this.Name = name;
            this.salary = salary;
        }

        public string Name { get; private set; }

        public void Add(IEmployee employee)
        {
            this.subordinates.Add(employee);
        }

        public void Remove(IEmployee employee)
        {
            this.subordinates.Remove(employee);
        }

        public decimal TotalSalary()
        {
            decimal ukupno = this.salary;
            foreach (IEmployee employee in this.subordinates)
            {
                ukupno += employee.TotalSalary();
            }
            return ukupno;
        }

        public int HeadCount()
        {
            int ukupno = 1;
            foreach (IEmployee employee in this.subordinates)
            {
                ukupno += employee.HeadCount();
            }
            return ukupno;
        }

        public int Depth()
        {
            int najdublje = 0;
            foreach (IEmployee employee in this.subordinates)
            {
                int dubina = employee.Depth();
                if (dubina > najdublje)
                {
                    najdublje = dubina;
                }
            }
            return najdublje + 1;
        }
    }
}
