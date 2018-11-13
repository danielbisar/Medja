using System.Text;

namespace Medja.Utils
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Removes the last character.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/>.</param>
        /// <returns>The same <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
        {
            return stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        public static char Last(this StringBuilder stringBuilder)
        {
            return stringBuilder[stringBuilder.Length - 1];
        }

        public static StringBuilder Prepend(this StringBuilder stringBuilder, char c)
        {
            return stringBuilder.Insert(0, c);
        }
    }
}