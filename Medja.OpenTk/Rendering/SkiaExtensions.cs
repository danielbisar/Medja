using System;
using Medja.Controls.Images;
using Medja.Primitives;
using SkiaSharp;

namespace Medja.OpenTk.Rendering
{
	/// <summary>
	/// Provides extensions to support the conversion and other things between Medja and Skia
	/// </summary>
	public static class SkiaExtensions
	{
		public static SKColor ToSKColor(this Color color)
		{
			if (color == null)
				return new SKColor(0, 0, 0, 0xFF);

			return new SKColor((byte)(0xFF * color.Red), (byte)(0xFF * color.Green), (byte)(0xFF * color.Blue));
		}

		/// <summary>
		/// Creates an SKPoint object from the X,Y components of the MPosition object.
		/// </summary>
		/// <returns>The SKPoint.</returns>
		/// <param name="position">Position.</param>
		public static SKPoint ToSKPoint(this MRect position)
		{
			return new SKPoint(position.X, position.Y);
		}

		/// <summary>
		/// Creates an SKRect from the MPosition object.
		/// </summary>
		/// <returns>The SKRect.</returns>
		/// <param name="position">Position.</param>
		public static SKRect ToSKRect(this MRect position)
		{
			return new SKRect(position.X, position.Y, position.Width + position.X, position.Height + position.Y);
		}

		public static SKColorType ToSkColorType(this PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.RGBA32:
					return SKColorType.Rgba8888;
				default:
					throw new ArgumentOutOfRangeException(nameof(pixelFormat), pixelFormat, null);
			}
		}
	}
}
