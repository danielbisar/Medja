namespace Medja.Utils.Text
{
    public static class StringExtensions
    {
        /// <summary>
        /// Finds the first index of the given char that is not inside "" or ''. 
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <param name="c">The char you are searching for.</param>
        /// <returns>-1 if the given char is not found in any part of the string not escaped by quotes.</returns>
        public static int IndexOfOutsideQuotes(this string str, char c)
        {
            var isInQuotes = false;
            var quoteStartChar = (char)0;

            for (int i = 0; i < str.Length; i++)
            {
                // quote ended
                if (isInQuotes && quoteStartChar == str[i])
                {
                    isInQuotes = false;
                }
                else if (str[i] == '"' || str[i] == '\'')
                {
                    quoteStartChar = str[i];
                    isInQuotes = true;
                }
                else if(!isInQuotes && str[i] == c)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}