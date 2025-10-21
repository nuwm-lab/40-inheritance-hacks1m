using System;
using System.Linq;

namespace LabWork.Geometry
{
    public class ConvexQuadrilateral : Shape
    {
        private readonly Point[] _vertices = new Point[4];

        public ConvexQuadrilateral(Point p1, Point p2, Point p3, Point p4)
        {
            SetVertices(p1, p2, p3, p4);
        }

        public ConvexQuadrilateral(Point[] pts)
        {
            if (pts == null) throw new ArgumentNullException(nameof(pts));
            if (pts.Length != 4) throw new ArgumentException("ConvexQuadrilateral requires exactly 4 points.", nameof(pts));
            SetVertices(pts[0], pts[1], pts[2], pts[3]);
        }

        public void SetVertices(Point p1, Point p2, Point p3, Point p4)
        {
            // Basic null/duplicate checks
            if (p1.Equals(p2) || p1.Equals(p3) || p1.Equals(p4) || p2.Equals(p3) || p2.Equals(p4) || p3.Equals(p4))
                throw new ArgumentException("Vertices must be distinct points.");

            _vertices[0] = p1; _vertices[1] = p2; _vertices[2] = p3; _vertices[3] = p4;

            // Ensure vertices are in correct order (convex and either clockwise or counter-clockwise)
            try
            {
                EnsureOrderAndConvexity();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[ConvexQuadrilateral] validation failed: {ex.Message}");
                throw new ArgumentException("Provided points do not form a valid convex quadrilateral.", ex);
            }
        }

        public override void PrintVertices()
        {
            Console.WriteLine("Convex quadrilateral vertices (ordered):");
            for (int i = 0; i < 4; i++) Console.WriteLine($"{i + 1}: {_vertices[i]}");
        }

        public override double Area()
        {
            // Shoelace for 4 points
            double sum = 0.0;
            for (int i = 0; i < 4; i++)
            {
                var a = _vertices[i];
                var b = _vertices[(i + 1) % 4];
                sum += a.X * b.Y - b.X * a.Y;
            }
            return 0.5 * Math.Abs(sum);
        }

        private void EnsureOrderAndConvexity()
        {
            // Compute centroid
            double cx = _vertices.Average(p => p.X);
            double cy = _vertices.Average(p => p.Y);
            // Sort by angle around centroid
            var ordered = _vertices.OrderBy(p => Math.Atan2(p.Y - cy, p.X - cx)).ToArray();
            for (int i = 0; i < 4; i++) _vertices[i] = ordered[i];

            // Check convexity: cross products of consecutive edges should have the same sign
            int sign = 0;
            for (int i = 0; i < 4; i++)
            {
                var a = _vertices[i];
                var b = _vertices[(i + 1) % 4];
                var c = _vertices[(i + 2) % 4];
                double cross = CrossProduct(a, b, c);
                int s = Math.Sign(cross);
                if (s == 0) throw new ArgumentException("Three consecutive vertices are colinear.");
                if (sign == 0) sign = s; else if (s != sign) throw new ArgumentException("Quadrilateral is not convex (self-intersecting or wrong order).");
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
