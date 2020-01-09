using Medja.Primitives;

namespace Medja.OpenTk.Themes.BlackRed
{
	public static class ThemeBlackRedValues
	{
		// utils: 
		// http://corecoding.com/utilities/rgb-or-hex-to-float.php
		// https://material.io/tools/color/

		public static readonly Color PrimaryColor = new Color(0.157f, 0.161f, 0.141f);
		public static readonly Color PrimaryLightColor = new Color(0.486f, 0.49f, 0.471f);
		public static readonly Color PrimaryTextColor = Colors.White;

		public static readonly Color SecondaryColor = new Color(0.659f, 0, 0);
		public static readonly Color SecondaryLightColor = SecondaryColor.GetDarker(0.7f);
		public static readonly Color SecondaryTextColor = Colors.White;
	}
}
