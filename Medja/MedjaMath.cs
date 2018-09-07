using System;
using Medja.Primitives;

namespace Medja
{
    public static class MedjaMath
    {
        public static bool AboutEquals(this double value1, double value2)
        {
            double epsilon = Math.Max(Math.Abs(value1), Math.Abs(value2)) * 1E-15;
            return Math.Abs(value1 - value2) <= epsilon;
        }

        public static float Distance(Point p1, Point p2)
        {
            var x = p1.X - p2.X;
            var y = p1.Y - p2.Y;

            return (float)Math.Sqrt(x * x + y * y);
        }
    }
}
