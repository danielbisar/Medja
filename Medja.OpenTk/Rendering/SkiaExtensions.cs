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

        public static SKColor ToSKColor(this Color color)
        {
            if (color == null)
                return new SKColor(0, 0, 0, 0xFF);

            return new SKColor((byte)(0xFF * color.Red), (byte)(0xFF * color.Green), (byte)(0xFF * color.Blue));
        }
    }
}
