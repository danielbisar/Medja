namespace Medja.Primitives
{
    /// <summary>
    /// This class contains methods to convert different Medja.Primitives to others. Since it is not the
    /// main resposability of a Primitiv to be convertable we implement the methods here.
    /// </summary>
    public static class PrimitivConverterExtensions
    {
        /// <summary>
        /// Converts the position (X, Y) of the <see cref="Rect"/> to a <see cref="Point"/> object.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/>.</param>
        /// <returns>The <see cref="Point"/>.</returns>
        public static Point ToPoint(this Rect rect)
        {
            return new Point(rect.X, rect.Y);
        }

        /// <summary>
        /// Creates a size object based on the width and height of the rects width and height.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/>.</param>
        /// <returns>The <see cref="Size"/>.</returns>
        public static Size ToSize(this Rect rect)
        {
            return new Size(rect.Width, rect.Height);
        }
    }
}