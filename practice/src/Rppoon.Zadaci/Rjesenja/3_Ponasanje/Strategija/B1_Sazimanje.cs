using System;

namespace Rppoon.Zadaci.Strategija.B1
{
    // ============ REFERENTNO RJESENJE - Strategija B1 ============

    public interface ICompressionStrategy
    {
        string Compress(string fileName);
    }

    /// <summary>
    /// Zajednicki dio obiju strategija izdvojen je ovdje - inace bi ista
    /// logika o nastavku stajala dvaput (DRY).
    /// </summary>
    internal static class Nastavak
    {
        public static string Zamijeni(string fileName, string noviNastavak)
        {
            int tocka = fileName.LastIndexOf('.');
            string osnova = tocka < 0 ? fileName : fileName.Substring(0, tocka);
            return osnova + "." + noviNastavak;
        }
    }

    public class ZipCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            return Nastavak.Zamijeni(fileName, "zip");
        }
    }

    public class RarCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            return Nastavak.Zamijeni(fileName, "rar");
        }
    }

    public class Archiver
    {
        public ICompressionStrategy Strategy { get; set; }

        public Archiver(ICompressionStrategy strategy)
        {
            this.Strategy = strategy;
        }

        public string Archive(string fileName)
        {
            return this.Strategy.Compress(fileName);
        }
    }
}
