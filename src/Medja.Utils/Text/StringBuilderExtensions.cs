using System.Text;

namespace Medja.Utils.Text
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Gets the last character of the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="System.Text.StringBuilder"/> instance.</param>
        /// <returns>The last character.</returns>
        public static char Last(this StringBuilder stringBuilder)
        {
            return stringBuilder[stringBuilder.Length - 1];
        }

        /// <summary>
        /// Inserts the given char at the beginning of the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="System.Text.StringBuilder"/> instance.</param>
        /// <param name="c">The character.</param>
        /// <returns>The given <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder Prepend(this StringBuilder stringBuilder, char c)
        {
            return stringBuilder.Insert(0, c);
        }
        
        /// <summary>
        /// Removes the last character from the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="System.Text.StringBuilder"/> instance.</param>
        /// <returns>The given <see cref="StringBuilder"/> instance.</returns>
        public static StringBuilder RemoveLast(this StringBuilder stringBuilder)
        {
            return stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
    }
}