using System;
using System.Globalization;

namespace LabWork.Geometry
{
    public readonly struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => X.ToString(CultureInfo.InvariantCulture) + ", " + Y.ToString(CultureInfo.InvariantCulture);

        // Read n points from console; throws if input ends unexpectedly
        public static Point[] ReadNPointsFromConsole(int n)
        {
            if (n <= 0) throw new ArgumentException("n must be positive", nameof(n));
            Point[] res = new Point[n];
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    Console.WriteLine($"Ввести координати точки #{i + 1} у форматі: x y");
                    Console.Write("> ");
                    string? line = Console.ReadLine();
                    if (line == null) throw new InvalidOperationException("Введено завершено користувачем");
                    string[] parts = line.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Потрібно ввести 2 числа (x y). Спробуйте ще раз.");
                        continue;
                    }
                    if (!double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                        !double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                    {
                        Console.WriteLine("Неможливо розпізнати числа. Використовуйте крапку як десятковий роздільник.");
                        continue;
                    }
                    res[i] = new Point(x, y);
                    break;
                }
            }
            return res;
        }

        public static void ReadThreePointsFromConsole(out Point p1, out Point p2, out Point p3)
        {
            var pts = ReadNPointsFromConsole(3);
            p1 = pts[0]; p2 = pts[1]; p3 = pts[2];
        }
    }
}
