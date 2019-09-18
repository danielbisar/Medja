namespace Medja.Primitives
{
    public static class Colors
    {
        // names according to https://graf1x.com/list-of-colors-with-color-names/
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color Blue = Color.FromHexStr("#0018F9");
        public static readonly Color Denim = Color.FromHexStr("#131E3A");
        public static readonly Color Forest = Color.FromHexStr("#0B6623");
        public static readonly Color Gold = Color.FromHexStr("#F9A602");
        public static readonly Color Green = Color.FromHexStr("#3BB143");
        public static readonly Color Gray = new Color(0.5f, 0.5f, 0.5f);
        public static readonly Color Hibiscus = Color.FromHexStr("#B43757");
        public static readonly Color LightGray = new Color(0.3f, 0.3f, 0.3f);
        public static readonly Color OrangeYellow = Color.FromHexStr("#FFCC00");
        public static readonly Color Pink = Color.FromHexStr("#FC0FC0");
        public static readonly Color Red = new Color(1, 0, 0);
        public static readonly Color Ruby = Color.FromHexStr("#E0115F");
        public static readonly Color White = new Color(1, 1, 1);
        public static readonly Color Yellow = Color.FromHexStr("#FC6600");

        public static readonly Color[] ColorsArray =
        {
            Black,
            Blue,
            Green,
            OrangeYellow,
            Gray,
            Denim,
            Ruby,
            Yellow
        };

        public static Color GetByIndex(int index)
        {
            return ColorsArray[index % ColorsArray.Length];
        }
    }
}
