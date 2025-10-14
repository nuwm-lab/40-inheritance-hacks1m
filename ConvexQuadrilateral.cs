using System;
using System.Linq;

namespace LabWork.Geometry
{
    public class ConvexQuadrilateral : IShape
    {
        private readonly Point[] _pts = new Point[4];

        public ConvexQuadrilateral(Point p1, Point p2, Point p3, Point p4)
        {
            SetVertices(p1, p2, p3, p4);
        }

        public ConvexQuadrilateral(Point[] pts)
        {
            if (pts == null || pts.Length != 4) throw new ArgumentException("Потрібно 4 точки");
            SetVertices(pts[0], pts[1], pts[2], pts[3]);
        }

        public void SetVertices(Point p1, Point p2, Point p3, Point p4)
        {
            _pts[0] = p1; _pts[1] = p2; _pts[2] = p3; _pts[3] = p4;
            // Ensure vertices are in correct order (convex and either clockwise or counter-clockwise)
            EnsureOrderAndConvexity();
        }

    public void PrintVertices()
        {
            Console.WriteLine("Вершини опуклого чотирикутника (після впорядкування):");
            for (int i = 0; i < 4; i++) Console.WriteLine($"{i + 1}: {_pts[i]}");
        }

    public double Area()
        {
            // Shoelace for 4 points
            double sum = 0.0;
            for (int i = 0; i < 4; i++)
            {
                var a = _pts[i];
                var b = _pts[(i + 1) % 4];
                sum += a.X * b.Y - b.X * a.Y;
            }
            return 0.5 * Math.Abs(sum);
        }

        private void EnsureOrderAndConvexity()
        {
            // Compute centroid
            double cx = _pts.Average(p => p.X);
            double cy = _pts.Average(p => p.Y);
            // Sort by angle around centroid
            var ordered = _pts.OrderBy(p => Math.Atan2(p.Y - cy, p.X - cx)).ToArray();
            for (int i = 0; i < 4; i++) _pts[i] = ordered[i];

            // Check convexity: cross products of consecutive edges should have the same sign
            int sign = 0;
            for (int i = 0; i < 4; i++)
            {
                var a = _pts[i];
                var b = _pts[(i + 1) % 4];
                var c = _pts[(i + 2) % 4];
                double cross = CrossProduct(a, b, c);
                int s = Math.Sign(cross);
                if (s == 0) throw new ArgumentException("Точки не можуть містити три послідовні колінеарні точки");
                if (sign == 0) sign = s; else if (s != sign) throw new ArgumentException("Чотирикутник не є опуклим або точки не в правильному порядку");
            }
        }

        private static double CrossProduct(Point a, Point b, Point c)
        {
            // cross of vectors AB x BC
            double abx = b.X - a.X;
            double aby = b.Y - a.Y;
            double bcx = c.X - b.X;
            double bcy = c.Y - b.Y;
            return abx * bcy - aby * bcx;
        }
    }
}
