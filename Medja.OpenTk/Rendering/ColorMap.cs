using Medja.Primitives;

namespace Medja.OpenTk.Rendering
{
	public static class ColorMap
	{
		// utils: 
		// http://corecoding.com/utilities/rgb-or-hex-to-float.php
		// https://material.io/tools/color/

		public static readonly Color Primary = new Color(0.157f, 0.161f, 0.141f);
		public static readonly Color PrimaryLight = new Color(0.486f, 0.49f, 0.471f);
		public static readonly Color PrimaryText = Colors.White;

		public static readonly Color Secondary = new Color(0.659f, 0, 0);
		public static readonly Color SecondaryLight = new Color(0.882f, 0.278f, 0.173f);
		public static readonly Color SecondaryText = Colors.White;
	}
}
