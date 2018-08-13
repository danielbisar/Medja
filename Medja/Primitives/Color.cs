using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Primitives
{
	public class Color
	{
		public float Red { get; }
		public float Green { get; }
		public float Blue { get; }

		public Color(float red, float green, float blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		/// <summary>
		/// Creates a new instance which is brighter by the given factor.
		/// </summary>
		/// <returns>The lighter color.</returns>
		/// <param name="factor">The value should be between 1 and 0.</param>
		public Color GetLighter(float factor)
		{
			return new Color((1 - Red) * factor, (1 - Green) * factor, (1 - Blue) * factor);
		}

		/// <summary>
		/// Creates a new instance which is darker by the given factor.
		/// </summary>
		/// <returns>The darker color.</returns>
		/// <param name="factor">Should be a value between 1 (no modifcation) - 0 (black).</param>
		public Color GetDarker(float factor)
		{
			return new Color(Red * factor, Green * factor, Blue * factor);
		}

		public override string ToString()
		{
			return "Red = " + Red + ", Green = " + Green + ", Blue = " + Blue;
		}
	}
}
