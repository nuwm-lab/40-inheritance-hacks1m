using System;
using System.Globalization;

namespace LabWork.Geometry
{
    public class Triangle : Shape
    {
        private const double EPS = 1e-12;

        private readonly Point _p1;
        private readonly Point _p2;
        private readonly Point _p3;

        public Triangle(Point p1, Point p2, Point p3)
        {
            _p1 = p1; _p2 = p2; _p3 = p3;
            double area = Area();
            if (area <= EPS)
                throw new ArgumentException("Degenerate triangle: points are colinear or area is too small.");
        }

        public override void PrintVertices()
        {
            Console.WriteLine("Triangle vertices:");
            Console.WriteLine($"A: {_p1}");
            Console.WriteLine($"B: {_p2}");
            Console.WriteLine($"C: {_p3}");
        }

        // Shoelace formula (determinant) for triangle
        public override double Area()
        {
            double x1 = _p1.X, y1 = _p1.Y;
            double x2 = _p2.X, y2 = _p2.Y;
            double x3 = _p3.X, y3 = _p3.Y;
            double val = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);
            return 0.5 * Math.Abs(val);
        }
    }
}
