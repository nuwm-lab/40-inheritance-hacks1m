using System;
using LabWork.Geometry;
using Xunit;

namespace LabWork.Tests
{
    public class TriangleQuadTests
    {
        [Fact]
        public void TriangleArea_HappyPath()
        {
            var a = new Point(0, 0);
            var b = new Point(1, 0);
            var c = new Point(0, 1);
            var t = new Triangle(a, b, c);
            Assert.Equal(0.5, t.Area(), 6);
        }

        [Fact]
        public void Triangle_Degenerate_Throws()
        {
            var a = new Point(0, 0);
            var b = new Point(1, 1);
            var c = new Point(2, 2);
            Assert.Throws<ArgumentException>(() => new Triangle(a, b, c));
        }

        [Fact]
        public void QuadrilateralArea_HappyPath()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 0);
            var p3 = new Point(1, 1);
            var p4 = new Point(0, 1);
            var q = new ConvexQuadrilateral(p1, p2, p3, p4);
            Assert.Equal(1.0, q.Area(), 6);
        }

        [Fact]
        public void Quadrilateral_NonConvex_Throws()
        {
            // self-intersecting (bow) quadrilateral
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var p3 = new Point(0, 1);
            var p4 = new Point(1, 0);
            Assert.Throws<ArgumentException>(() => new ConvexQuadrilateral(p1, p2, p3, p4));
        }
    }
}
