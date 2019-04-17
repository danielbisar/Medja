using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Utils
{
    /// <summary>
    /// Some extensions for the Random class.
    /// </summary>
    public static class RandomExtensions
    {
        public static readonly char[] WordCharsAndNumbers =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        
        /// <summary>
        /// Returns a random <see cref="string"/>.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="length">The length of the resulting <see cref="string"/>.</param>
        /// <returns>A random <see cref="string"/>.</returns>
        /// <remarks><see cref="WordCharsAndNumbers"/> is used to generate the <see cref="string"/>.</remarks>
        public static string NextString(this Random random, int length)
        {
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var charIndex = random.Next(WordCharsAndNumbers.Length - 1);
                result.Append(WordCharsAndNumbers[charIndex]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets a random item from the given list.
        /// </summary>
        /// <param name="random">The Random instance.</param>
        /// <param name="list">The list of items to use.</param>
        /// <returns>Any item from list.</returns>
        public static T NextItem<T>(this Random random, IReadOnlyList<T> list)
        {
            return list[random.Next(list.Count - 1)];
        }
    }
}