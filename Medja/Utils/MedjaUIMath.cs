using Medja.Primitives;

namespace Medja.Utils
{
    public static class MedjaUIMath
    {
        /// <summary>
        /// Gets the distance of two points.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float Distance(this Point p1, Point p2)
        {
            var x = p1.X - p2.X;
            var y = p1.Y - p2.Y;

            return (float)System.Math.Sqrt(x * x + y * y);
        }
    }
}