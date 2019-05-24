using System.Drawing;

namespace Medja.OpenTk.Utils
{
    public static class PrimitivConverterExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.Point"/> to <see cref="Medja.Primitives.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="System.Drawing.Point"/>.</param>
        /// <returns>The <see cref="Medja.Primitives.Point"/>.</returns>
        public static Medja.Primitives.Point ToMedjaPoint(this Point point)
        {
            return new Primitives.Point(point.X, point.Y);
        }
    }
}