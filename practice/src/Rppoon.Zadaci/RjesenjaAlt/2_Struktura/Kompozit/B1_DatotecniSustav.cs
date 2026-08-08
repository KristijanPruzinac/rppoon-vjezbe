using System;
using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.B1
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit B1 =====
    //
    // Zbrajanje LINQ-om, pretraga preko Select + FirstOrDefault umjesto
    // rucne petlje s ranim izlazom, ime u izricitom polju.

    public interface IFileSystemItem
    {
        string Name { get; }
        int Size();
        int FileCount();
        IFileSystemItem Find(string name);
    }

    public class File : IFileSystemItem
    {
        private readonly string name;
        private readonly int sizeKb;

        public File(string name, int sizeKb)
        {
            this.name = name;
            this.sizeKb = sizeKb;
        }

        public string Name => this.name;

        public int Size() => this.sizeKb;

        public int FileCount() => 1;

        public IFileSystemItem Find(string name) => this.name == name ? this : null;
    }

    public class Folder : IFileSystemItem
    {
        private readonly string name;
        private readonly List<IFileSystemItem> sadrzaj = new List<IFileSystemItem>();

        public Folder(string name)
        {
            this.name = name;
        }

        public string Name => this.name;

        public void Add(IFileSystemItem item) => this.sadrzaj.Add(item);

        public void Remove(IFileSystemItem item) => this.sadrzaj.Remove(item);

        public int Size() => this.sadrzaj.Sum(s => s.Size());

        public int FileCount() => this.sadrzaj.Sum(s => s.FileCount());

        public IFileSystemItem Find(string name)
        {
            if (this.name == name)
            {
                return this;
            }

            return this.sadrzaj
                .Select(s => s.Find(name))
                .FirstOrDefault(nadeno => nadeno != null);
        }
    }
}
