using System.Collections.Generic;

namespace Rppoon.Zadaci.Kompozit.C1
{
    // ============ REFERENTNO RJESENJE - Kompozit C1 ============

    public interface IShape
    {
        string Draw();
        double Left();
        double Right();
        double Top();
    }

    public class Circle : IShape
    {
        private readonly double centerX;
        private readonly double centerY;
        private readonly double radius;

        public Circle(double centerX, double centerY, double radius)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.radius = radius;
        }

        public string Draw()
        {
            return "krug";
        }

        public double Left()
        {
            return this.centerX - this.radius;
        }

        public double Right()
        {
            return this.centerX + this.radius;
        }

        public double Top()
        {
            return this.centerY + this.radius;
        }
    }

    public class Rectangle : IShape
    {
        private readonly double x;
        private readonly double y;
        private readonly double width;
        private readonly double height;

        public Rectangle(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public string Draw()
        {
            return "pravokutnik";
        }

        public double Left()
        {
            return this.x;
        }

        public double Right()
        {
            return this.x + this.width;
        }

        public double Top()
        {
            return this.y + this.height;
        }
    }

    public class CompositeShape : IShape
    {
        private readonly List<IShape> shapes = new List<IShape>();

        public void Add(IShape shape)
        {
            this.shapes.Add(shape);
        }

        public void Remove(IShape shape)
        {
            this.shapes.Remove(shape);
        }

        public string Draw()
        {
            List<string> opisi = new List<string>();
            foreach (IShape shape in this.shapes)
            {
                opisi.Add(shape.Draw());
            }
            return string.Join("+", opisi);
        }

        public double Left()
        {
            if (this.shapes.Count == 0)
            {
                return 0;
            }

            double najmanji = this.shapes[0].Left();
            foreach (IShape shape in this.shapes)
            {
                if (shape.Left() < najmanji)
                {
                    najmanji = shape.Left();
                }
            }
            return najmanji;
        }

        public double Right()
        {
            if (this.shapes.Count == 0)
            {
                return 0;
            }

            double najveci = this.shapes[0].Right();
            foreach (IShape shape in this.shapes)
            {
                if (shape.Right() > najveci)
                {
                    najveci = shape.Right();
                }
            }
            return najveci;
        }

        public double Top()
        {
            if (this.shapes.Count == 0)
            {
                return 0;
            }

            double najveci = this.shapes[0].Top();
            foreach (IShape shape in this.shapes)
            {
                if (shape.Top() > najveci)
                {
                    najveci = shape.Top();
                }
            }
            return najveci;
        }
    }
}
