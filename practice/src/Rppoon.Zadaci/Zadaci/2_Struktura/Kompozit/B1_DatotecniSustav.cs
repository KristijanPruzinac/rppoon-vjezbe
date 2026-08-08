using System;

namespace Rppoon.Zadaci.Kompozit.B1
{
    // ============================================================
    //  KOMPOZIT - razina B (sastavi), zadatak 1: Datotecni sustav
    // ============================================================
    //  Dobivas sucelje i potpise. Sve ostalo pises sam.
    //
    //  Trazi se:
    //    File(name, sizeKb)        list
    //    Folder(name)              kompozit; Add / Remove
    //    Size()                    datoteka: vlastita velicina
    //                              mapa: zbroj svega unutra
    //    FileCount()               koliko DATOTEKA ima u podstablu
    //                              (mape se ne broje)
    //    Find(name)                vraca prvu stavku tog imena u
    //                              podstablu ili null; mapa provjerava
    //                              prvo sebe, pa svoju djecu
    //
    //  Find je razlog zasto obrazac vrijedi: isti poziv pretrazuje i
    //  jednu datoteku i cijelo stablo od tisucu mapa.
    // ============================================================

    public interface IFileSystemItem
    {
        string Name { get; }
        int Size();
        int FileCount();
        IFileSystemItem Find(string name);
    }

    public class File : IFileSystemItem
    {
        public File(string name, int sizeKb)
        {
            throw new NotImplementedException("File konstruktor");
        }

        public string Name
        {
            get { throw new NotImplementedException("File.Name"); }
        }

        public int Size()
        {
            throw new NotImplementedException("File.Size");
        }

        public int FileCount()
        {
            throw new NotImplementedException("File.FileCount");
        }

        public IFileSystemItem Find(string name)
        {
            throw new NotImplementedException("File.Find");
        }
    }

    public class Folder : IFileSystemItem
    {
        public Folder(string name)
        {
            throw new NotImplementedException("Folder konstruktor");
        }

        public string Name
        {
            get { throw new NotImplementedException("Folder.Name"); }
        }

        public void Add(IFileSystemItem item)
        {
            throw new NotImplementedException("Folder.Add");
        }

        public void Remove(IFileSystemItem item)
        {
            throw new NotImplementedException("Folder.Remove");
        }

        public int Size()
        {
            throw new NotImplementedException("Folder.Size");
        }

        public int FileCount()
        {
            throw new NotImplementedException("Folder.FileCount");
        }

        public IFileSystemItem Find(string name)
        {
            throw new NotImplementedException("Folder.Find");
        }
    }
}
