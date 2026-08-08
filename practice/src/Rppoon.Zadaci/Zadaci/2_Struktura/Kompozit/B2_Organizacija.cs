using System;

namespace Rppoon.Zadaci.Kompozit.B2
{
    // ============================================================
    //  KOMPOZIT - razina B (sastavi), zadatak 2: Ustroj tvrtke
    // ============================================================
    //  Dobivas sucelje i potpise. Sve ostalo pises sam.
    //
    //  Razlika u odnosu na B1: ovdje operacije NISU sve zbrajanje.
    //    TotalSalary()  jest zbroj
    //    HeadCount()    jest zbroj, ali voditelj broji i SEBE
    //    Depth()        nije zbroj nego MAKSIMUM po djeci + 1
    //
    //  Ako pokusas sve rijesiti istim obrascem zbrajanja, Depth ce dati
    //  krivi broj. To je poanta zadatka.
    //
    //  Trazi se:
    //    Developer(name, salary)   list
    //    Manager(name, salary)     kompozit; Add / Remove
    //    TotalSalary()             razvijatelj: svoja placa
    //                              voditelj: svoja placa + sve ispod
    //    HeadCount()               razvijatelj: 1
    //                              voditelj: 1 + svi ispod
    //    Depth()                   razvijatelj: 1
    //                              voditelj: 1 + najveca dubina djeteta
    //                              (voditelj bez podredenih ima dubinu 1)
    // ============================================================

    public interface IEmployee
    {
        string Name { get; }
        decimal TotalSalary();
        int HeadCount();
        int Depth();
    }

    public class Developer : IEmployee
    {
        public Developer(string name, decimal salary)
        {
            throw new NotImplementedException("Developer konstruktor");
        }

        public string Name
        {
            get { throw new NotImplementedException("Developer.Name"); }
        }

        public decimal TotalSalary()
        {
            throw new NotImplementedException("Developer.TotalSalary");
        }

        public int HeadCount()
        {
            throw new NotImplementedException("Developer.HeadCount");
        }

        public int Depth()
        {
            throw new NotImplementedException("Developer.Depth");
        }
    }

    public class Manager : IEmployee
    {
        public Manager(string name, decimal salary)
        {
            throw new NotImplementedException("Manager konstruktor");
        }

        public string Name
        {
            get { throw new NotImplementedException("Manager.Name"); }
        }

        public void Add(IEmployee employee)
        {
            throw new NotImplementedException("Manager.Add");
        }

        public void Remove(IEmployee employee)
        {
            throw new NotImplementedException("Manager.Remove");
        }

        public decimal TotalSalary()
        {
            throw new NotImplementedException("Manager.TotalSalary");
        }

        public int HeadCount()
        {
            throw new NotImplementedException("Manager.HeadCount");
        }

        public int Depth()
        {
            throw new NotImplementedException("Manager.Depth");
        }
    }
}
