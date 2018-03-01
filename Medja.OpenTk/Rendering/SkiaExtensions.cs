using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
    /// <summary>
    /// Provides extensions to support the conversion and other things between Medja and Skia
    /// </summary>
    public static class SkiaExtensions
    {
        public static SKRect ToSKRect(this MPosition position)
        {
            return new SKRect(position.X, position.Y, position.Width + position.X, position.Height + position.Y);
        }
    }
}
