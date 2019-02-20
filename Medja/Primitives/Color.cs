using System;
using System.Globalization;

namespace Medja.Primitives
{
	public class Color
	{
		public static Color FromHexStr(string colorValues)
		{
			if(colorValues.Length != 7 && colorValues.Length != 6)
				throw new ArgumentException("Color values need to be in the format #RRGGBB or RRGGBB", nameof(colorValues));

			var i = colorValues.Length - 6;

			var r = int.Parse(colorValues.Substring(i, 2), NumberStyles.HexNumber);
			var g = int.Parse(colorValues.Substring(i+2, 2), NumberStyles.HexNumber);
			var b = int.Parse(colorValues.Substring(i+4, 2), NumberStyles.HexNumber);
			
			// todo implement alpha channel
			
			return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
		}
		
		public float Red { get; }
		public float Green { get; }
		public float Blue { get; }
		public float Alpha { get; }

		public Color(float red, float green, float blue, float alpha = 1.0f)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		/// <summary>
		/// Creates a new instance which is brighter by the given factor.
		/// </summary>
		/// <returns>The lighter color.</returns>
		/// <param name="factor">The value should be between 1 and 0.</param>
		public Color GetLighter(float factor)
		{
			return new Color((1 - Red) * factor, (1 - Green) * factor, (1 - Blue) * factor, Alpha);
		}

		/// <summary>
		/// Creates a new instance which is darker by the given factor.
		/// </summary>
		/// <returns>The darker color.</returns>
		/// <param name="factor">Should be a value between 1 (no modifcation) - 0 (black).</param>
		public Color GetDarker(float factor)
		{
			return new Color(Red * factor, Green * factor, Blue * factor, Alpha);
		}

		/// <summary>
		/// Gets the colors disabled state according to https://material.io/design/interaction/states.html#disabled
		/// </summary>
		/// <returns></returns>
		public Color GetDisabled()
		{
			return new Color(Red, Green, Blue, Alpha * 0.38f);
		}

		public override string ToString()
		{
			return "Red = " + Red + ", Green = " + Green + ", Blue = " + Blue;
		}
	}
}
