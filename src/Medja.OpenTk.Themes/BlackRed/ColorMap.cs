using Medja.Primitives;

namespace Medja.OpenTk.Themes.BlackRed
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
		public static readonly Color SecondaryLight = Secondary.GetDarker(0.7f);
		public static readonly Color SecondaryText = Colors.White;
	}
}
