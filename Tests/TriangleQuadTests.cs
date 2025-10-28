using System;
using Xunit;

namespace LabWork.Tests
{
    public class TriangleQuadTests
    {
        [Fact]
        public void TriangleArea_HappyPath()
        {
            var a = new Point { X = 0, Y = 0 };
            var b = new Point { X = 1, Y = 0 };
            var c = new Point { X = 0, Y = 1 };
            var t = new Triangle(a, b, c);
            Assert.Equal(0.5, t.CalculateArea(), 6);
        }

        [Fact]
        public void QuadrilateralArea_HappyPath()
        {
            var p1 = new Point { X = 0, Y = 0 };
            var p2 = new Point { X = 1, Y = 0 };
            var p3 = new Point { X = 1, Y = 1 };
            var p4 = new Point { X = 0, Y = 1 };
            var q = new ConvexQuadrilateral(p1, p2, p3, p4);
            Assert.Equal(1.0, q.CalculateArea(), 6);
        }
    }
}
