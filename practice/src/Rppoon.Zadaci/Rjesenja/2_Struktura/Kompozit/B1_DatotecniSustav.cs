using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.B1
{
    // ============ REFERENTNO RJESENJE - Kompozit B1 ============

    public interface IFileSystemItem
    {
        string Name { get; }
        int Size();
        int FileCount();
        IFileSystemItem Find(string name);
    }

    public class File : IFileSystemItem
    {
        private readonly int sizeKb;

        public File(string name, int sizeKb)
        {
            this.Name = name;
            this.sizeKb = sizeKb;
        }

        public string Name { get; private set; }

        public int Size()
        {
            return this.sizeKb;
        }

        public int FileCount()
        {
            return 1;
        }

        public IFileSystemItem Find(string name)
        {
            if (this.Name == name)
            {
                return this;
            }
            return null;
        }
    }

    public class Folder : IFileSystemItem
    {
        private readonly List<IFileSystemItem> items = new List<IFileSystemItem>();

        public Folder(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public void Add(IFileSystemItem item)
        {
            this.items.Add(item);
        }

        public void Remove(IFileSystemItem item)
        {
            this.items.Remove(item);
        }

        public int Size()
        {
            int ukupno = 0;
            foreach (IFileSystemItem item in this.items)
            {
                ukupno += item.Size();
            }
            return ukupno;
        }

        public int FileCount()
        {
            int ukupno = 0;
            foreach (IFileSystemItem item in this.items)
            {
                ukupno += item.FileCount();
            }
            return ukupno;
        }

        public IFileSystemItem Find(string name)
        {
            if (this.Name == name)
            {
                return this;
            }

            foreach (IFileSystemItem item in this.items)
            {
                IFileSystemItem nadeno = item.Find(name);
                if (nadeno != null)
                {
                    return nadeno;
                }
            }

            return null;
        }
    }
}
