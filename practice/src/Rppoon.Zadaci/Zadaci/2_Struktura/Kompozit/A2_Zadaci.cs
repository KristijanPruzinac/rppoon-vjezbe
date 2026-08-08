using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.A2
{
    // ============================================================
    //  KOMPOZIT - razina A (vodeno), zadatak 2: Popis zadataka
    // ============================================================
    //  Ovdje je kompozit (Project) gotov, a ti pises list (Task) i
    //  procjenu vremena.
    //
    //  Projekt se sastoji od zadataka i drugih projekata. Procjena
    //  vremena za projekt je zbroj procjena svega sto sadrzi.
    //  Broj dovrsenih stavki broji se kroz cijelo stablo.
    // ============================================================

    public interface IToDoItem
    {
        string Title { get; }

        /// <summary>Procjena u satima.</summary>
        int EstimateHours();

        /// <summary>Koliko je stavki u ovom podstablu dovrseno.</summary>
        int CompletedCount();
    }

    /// <summary>List - pojedinacni zadatak.</summary>
    public class Task : IToDoItem
    {
        private readonly int hours;
        private readonly bool completed;

        public Task(string title, int hours, bool completed)
        {
            this.Title = title;
            this.hours = hours;
            this.completed = completed;
        }

        public string Title { get; private set; }

        public int EstimateHours()
        {
            // TODO: procjena pojedinacnog zadatka je njegov broj sati.
            throw new NotImplementedException("Task.EstimateHours");
        }

        public int CompletedCount()
        {
            // TODO: 1 ako je zadatak dovrsen, inace 0.
            throw new NotImplementedException("Task.CompletedCount");
        }
    }

    /// <summary>Kompozit. DAN JE U CIJELOSTI - promotri kako se rekurzija dogada sama.</summary>
    public class Project : IToDoItem
    {
        private readonly List<IToDoItem> items = new List<IToDoItem>();

        public Project(string title)
        {
            this.Title = title;
        }

        public string Title { get; private set; }

        public int Count
        {
            get { return this.items.Count; }
        }

        public void Add(IToDoItem item)
        {
            this.items.Add(item);
        }

        public void Remove(IToDoItem item)
        {
            this.items.Remove(item);
        }

        public int EstimateHours()
        {
            int ukupno = 0;
            foreach (IToDoItem item in this.items)
            {
                ukupno += item.EstimateHours();
            }
            return ukupno;
        }

        public int CompletedCount()
        {
            int ukupno = 0;
            foreach (IToDoItem item in this.items)
            {
                ukupno += item.CompletedCount();
            }
            return ukupno;
        }
    }
}
