using System;
using Medja.Primitives;
using OpenTK;
using SkiaSharp;

namespace Medja.OpenTk.Utils
{
    public static class PrimitiveConverterExtensions
    {
        /// <summary>
        /// Gets the <see cref="SKFontStyle"/> from the given <see cref="Font"/>.
        /// </summary>
        /// <param name="font">The font object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If font.Style has an unknown value.</exception>
        public static SKFontStyle GetSKFontStyle(this Font font)
        {
            switch (font.Style)
            {
                case FontStyle.Normal:
                    return SKFontStyle.Normal;
                case FontStyle.Bold:
                    return SKFontStyle.Bold;
                case FontStyle.Italic:
                    return SKFontStyle.Italic;
                case FontStyle.BoldAndItalic:
                    return SKFontStyle.BoldItalic;
                default:
                    throw new ArgumentOutOfRangeException(nameof(font.Style));
            }
        }
        
        /// <summary>
        /// Converts a <see cref="System.Drawing.Point"/> to <see cref="Medja.Primitives.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="System.Drawing.Point"/>.</param>
        /// <returns>The <see cref="Medja.Primitives.Point"/>.</returns>
        public static Point ToMedjaPoint(this System.Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts a <see cref="Controls.WindowState"/> to <see cref="WindowState"/>.
        /// </summary>
        /// <param name="windowState">The <see cref="Controls.WindowState"/>.</param>
        /// <returns>The <see cref="WindowState"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static WindowState ToOpenTKWindowState(this Controls.WindowState windowState)
        {
            return windowState switch
            {
                Controls.WindowState.Normal => WindowState.Normal,
                Controls.WindowState.Fullscreen => WindowState.Fullscreen,
                _ => throw new ArgumentOutOfRangeException(nameof(windowState))
            };
        }
    }
}