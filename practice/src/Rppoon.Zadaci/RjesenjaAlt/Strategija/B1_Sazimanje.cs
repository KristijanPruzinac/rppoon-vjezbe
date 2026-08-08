using System;

namespace Rppoon.Zadaci.Strategija.B1
{
    // ===== ALTERNATIVNO RJESENJE - Strategija B1 =====
    //
    // Bez zajednickog pomocnog razreda: svaka strategija sama slaze naziv,
    // a Archiver drzi strategiju u izricitom polju.

    public interface ICompressionStrategy
    {
        string Compress(string fileName);
    }

    public class ZipCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            string[] dijelovi = fileName.Split('.');
            if (dijelovi.Length == 1)
            {
                return fileName + ".zip";
            }
            return string.Join(".", dijelovi, 0, dijelovi.Length - 1) + ".zip";
        }
    }

    public class RarCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            string[] dijelovi = fileName.Split('.');
            if (dijelovi.Length == 1)
            {
                return fileName + ".rar";
            }
            return string.Join(".", dijelovi, 0, dijelovi.Length - 1) + ".rar";
        }
    }

    public class Archiver
    {
        private ICompressionStrategy strategy;

        public Archiver(ICompressionStrategy strategy)
        {
            this.strategy = strategy;
        }

        public ICompressionStrategy Strategy
        {
            get { return this.strategy; }
            set { this.strategy = value; }
        }

        public string Archive(string fileName)
        {
            return this.strategy.Compress(fileName);
        }
    }
}
