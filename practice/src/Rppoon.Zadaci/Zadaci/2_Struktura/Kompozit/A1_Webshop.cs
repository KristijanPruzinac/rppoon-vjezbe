using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.A1
{
    // ============================================================
    //  KOMPOZIT - razina A (vodeno), zadatak 1: Poklon-paket
    // ============================================================
    //  List (Product) je gotov. Ti pises kompozit (GiftSet).
    //
    //  Poanta: klijent poziva CalculatePriceWithTax() i NE ZNA drzi li
    //  u ruci jedan proizvod ili paket koji sadrzi druge pakete.
    //  Jedan te isti poziv radi na oba.
    //
    //  Napomena o inacici: Add i Remove stoje SAMO na kompozitu, ne na
    //  sucelju IBuyable. To je "sigurna" inacica obrasca - proizvod
    //  nema besmislenu metodu Add. Cijena je da klijent mora znati
    //  radi li s kompozitom kad zeli mijenjati sadrzaj.
    // ============================================================

    /// <summary>Komponenta - zajednicko sucelje lista i kompozita.</summary>
    public interface IBuyable
    {
        string Name { get; }
        decimal CalculatePriceWithTax();
    }

    /// <summary>List. DAN JE U CIJELOSTI. Porez je 25 %.</summary>
    public class Product : IBuyable
    {
        private readonly decimal price;

        public Product(string name, decimal price)
        {
            this.Name = name;
            this.price = price;
        }

        public string Name { get; private set; }

        public decimal CalculatePriceWithTax()
        {
            return this.price * 1.25m;
        }
    }

    /// <summary>Kompozit - paket koji sadrzi druge kupovne stavke.</summary>
    public class GiftSet : IBuyable
    {
        private readonly List<IBuyable> items = new List<IBuyable>();

        public GiftSet(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        /// <summary>Koliko stavki paket izravno sadrzi (bez ulazenja dublje).</summary>
        public int Count
        {
            get { return this.items.Count; }
        }

        public void Add(IBuyable item)
        {
            // TODO: dodaj stavku u paket.
            throw new NotImplementedException("GiftSet.Add");
        }

        public void Remove(IBuyable item)
        {
            // TODO: makni stavku iz paketa.
            throw new NotImplementedException("GiftSet.Remove");
        }

        public decimal CalculatePriceWithTax()
        {
            // TODO: zbroji cijene svih sadrzanih stavki.
            //       Ne razlikuj proizvod od paketa - obje su IBuyable.
            throw new NotImplementedException("GiftSet.CalculatePriceWithTax");
        }
    }
}
