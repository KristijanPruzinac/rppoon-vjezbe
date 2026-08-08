using System;

namespace Rppoon.Zadaci.Strategija.B1
{
    // ============================================================
    //  STRATEGIJA - razina B (sastavi), zadatak 1: Sazimanje datoteka
    // ============================================================
    //  Dobivas SAMO sucelje i potpise. Tijela su tvoja, ukljucujuci
    //  i kontekst Archiver - on jos nema nista osim imena.
    //
    //  Trazi se:
    //    ZipCompression.Compress("izvjestaj.txt")  -> "izvjestaj.zip"
    //    RarCompression.Compress("izvjestaj.txt")  -> "izvjestaj.rar"
    //    Archiver mijenja nacin sazimanja u hodu, bez ijednog if-a.
    //
    //  Naziv bez tocke ("podaci") dobiva nastavak na kraj: "podaci.zip".
    // ============================================================

    public interface ICompressionStrategy
    {
        string Compress(string fileName);
    }

    public class ZipCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            throw new NotImplementedException("ZipCompression.Compress");
        }
    }

    public class RarCompression : ICompressionStrategy
    {
        public string Compress(string fileName)
        {
            throw new NotImplementedException("RarCompression.Compress");
        }
    }

    /// <summary>
    /// Kontekst. Mora drzati strategiju, dopustiti zamjenu i delegirati joj posao.
    /// </summary>
    public class Archiver
    {
        public ICompressionStrategy Strategy { get; set; }

        public Archiver(ICompressionStrategy strategy)
        {
            throw new NotImplementedException("Archiver konstruktor");
        }

        public string Archive(string fileName)
        {
            throw new NotImplementedException("Archiver.Archive");
        }
    }
}
