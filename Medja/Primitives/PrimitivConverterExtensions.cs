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
    }
}