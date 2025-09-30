using System;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 
    public class Polygon
    {
        protected (double x, double y)[] vertices;
        public virtual void SetVertices(params (double x, double y)[] coords)
        {
            vertices = coords;
        }
        public virtual void PrintVertices()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Console.WriteLine($"Vertex {i + 1}: ({vertices[i].x}, {vertices[i].y})");
            }
        }
        public virtual double Area()
        {
            // Shoelace formula for convex polygons
            double area = 0;
            int n = vertices.Length;
            for (int i = 0; i < n; i++)
            {
                var (x1, y1) = vertices[i];
                var (x2, y2) = vertices[(i + 1) % n];
                area += (x1 * y2 - x2 * y1);
            }
            return Math.Abs(area) / 2.0;
        }
    }

    public class Triangle : Polygon
    {
        public override void SetVertices(params (double x, double y)[] coords)
        {
            if (coords.Length != 3)
                throw new ArgumentException("Triangle must have 3 vertices.");
            base.SetVertices(coords);
        }
        public override void PrintVertices()
        {
            Console.WriteLine("Triangle vertices:");
            base.PrintVertices();
        }
        public override double Area()
        {
            return base.Area();
        }
    }

    public class ConvexQuadrilateral : Polygon
    {
        public override void SetVertices(params (double x, double y)[] coords)
        {
            if (coords.Length != 4)
                throw new ArgumentException("Quadrilateral must have 4 vertices.");
            base.SetVertices(coords);
        }
        public override void PrintVertices()
        {
            Console.WriteLine("Convex Quadrilateral vertices:");
            base.PrintVertices();
        }
        public override double Area()
        {
            return base.Area();
        }
    }

    class Delta
    {
        static void Main(string[] args)
        {
            // Трикутник
            Triangle triangle = new Triangle();
            triangle.SetVertices((0, 0), (4, 0), (0, 3));
            triangle.PrintVertices();
            Console.WriteLine($"Triangle area: {triangle.Area()}");

            Console.WriteLine();

            // Опуклий чотирикутник
            ConvexQuadrilateral quad = new ConvexQuadrilateral();
            quad.SetVertices((0, 0), (4, 0), (5, 3), (1, 4));
            quad.PrintVertices();
            Console.WriteLine($"Convex Quadrilateral area: {quad.Area()}");
        }
    }
}
