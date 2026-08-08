using System.Collections.Generic;
using System.Linq;

namespace Rppoon.Zadaci.Kompozit.C1
{
    // ===== ALTERNATIVNO RJESENJE - Kompozit C1 =====
    //
    // Krajnje tocke preko LINQ-a s DefaultIfEmpty(0), opis preko
    // string.Join nad Select, koordinate u automatskim svojstvima.

    public interface IShape
    {
        string Draw();
        double Left();
        double Right();
        double Top();
    }

    public class Circle : IShape
    {
        public Circle(double centerX, double centerY, double radius)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;
            this.Radius = radius;
        }

        public double CenterX { get; private set; }

        public double CenterY { get; private set; }

        public double Radius { get; private set; }

        public string Draw() => "krug";

        public double Left() => this.CenterX - this.Radius;

        public double Right() => this.CenterX + this.Radius;

        public double Top() => this.CenterY + this.Radius;
    }

    public class Rectangle : IShape
    {
        public Rectangle(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public string Draw() => "pravokutnik";

        public double Left() => this.X;

        public double Right() => this.X + this.Width;

        public double Top() => this.Y + this.Height;
    }

    public class CompositeShape : IShape
    {
        private readonly List<IShape> djeca = new List<IShape>();

        public void Add(IShape shape) => this.djeca.Add(shape);

        public void Remove(IShape shape) => this.djeca.Remove(shape);

        public string Draw() => string.Join("+", this.djeca.Select(d => d.Draw()));

        public double Left() => this.djeca.Select(d => d.Left()).DefaultIfEmpty(0).Min();

        public double Right() => this.djeca.Select(d => d.Right()).DefaultIfEmpty(0).Max();

        public double Top() => this.djeca.Select(d => d.Top()).DefaultIfEmpty(0).Max();
    }
}
