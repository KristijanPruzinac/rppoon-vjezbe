using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.A2
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit A2 =====
    //
    // List cuva stanje u automatskim svojstvima, dovrsenost pretvara
    // ternarnim izrazom, kompozit zbraja LINQ-om.

    public interface IToDoItem
    {
        string Title { get; }
        int EstimateHours();
        int CompletedCount();
    }

    public class Task : IToDoItem
    {
        public Task(string title, int hours, bool completed)
        {
            this.Title = title;
            this.Hours = hours;
            this.IsCompleted = completed;
        }

        public string Title { get; private set; }

        public int Hours { get; private set; }

        public bool IsCompleted { get; private set; }

        public int EstimateHours() => this.Hours;

        public int CompletedCount() => this.IsCompleted ? 1 : 0;
    }

    public class Project : IToDoItem
    {
        private readonly List<IToDoItem> stavke = new List<IToDoItem>();

        public Project(string title)
        {
            this.Title = title;
        }

        public string Title { get; private set; }

        public int Count => this.stavke.Count;

        public void Add(IToDoItem item) => this.stavke.Add(item);

        public void Remove(IToDoItem item) => this.stavke.Remove(item);

        public int EstimateHours() => this.stavke.Sum(s => s.EstimateHours());

        public int CompletedCount() => this.stavke.Sum(s => s.CompletedCount());
    }
}
