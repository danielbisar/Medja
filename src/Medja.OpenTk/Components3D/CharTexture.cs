using System.Linq;

namespace Medja.OpenTk.Components3D
{
    public class CharTexture
    {
        public static readonly string Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÜ!\"§$%&/()=?{[]}\\^°`´@€*+~'#;,:._-<>|";
        public readonly CharTextureCoordinate[] Coordinates;

        public CharTexture()
        {
            Coordinates = new CharTextureCoordinate[Chars.Max()];
        }

        public CharTextureCoordinate GetCoordinates(char c)
        {
            return Coordinates[c];
        }
    }
}