using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.A2
{
    // ============ REFERENTNO RJESENJE - Kompozit A2 ============

    public interface IToDoItem
    {
        string Title { get; }
        int EstimateHours();
        int CompletedCount();
    }

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
            return this.hours;
        }

        public int CompletedCount()
        {
            if (this.completed)
            {
                return 1;
            }
            return 0;
        }
    }

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
