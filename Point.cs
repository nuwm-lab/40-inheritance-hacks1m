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
                Console.WriteLine($"Enter coordinates for point #{i + 1} (x y) using dot as decimal separator:");
                double[]? coords = Program.ReadDoublesFromConsole(2, "");
                if (coords == null) throw new InvalidOperationException("Input ended unexpectedly");
                res[i] = new Point(coords[0], coords[1]);
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
