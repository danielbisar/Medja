namespace Medja.Primitives
{
	public static class Colors
	{
		public static readonly Color Black = new Color(0, 0, 0);
		public static readonly Color Blue = new Color(0, 0, 1);
		public static readonly Color Green = new Color(0, 1, 0);
		public static readonly Color Gray = new Color(0.5f, 0.5f, 0.5f);
		public static readonly Color LightGray = new Color(0.3f, 0.3f, 0.3f);
		public static readonly Color Red = new Color(1, 0, 0);
		public static readonly Color White = new Color(1, 1, 1);

		public static readonly Color[] ColorsArray =
		{
			Black,
			Blue,
			Green,
			Gray,
			LightGray,
			Red,
			White
		};

		public static Color GetByIndex(int index)
		{
			return ColorsArray[index % ColorsArray.Length];
		}
	}
}
