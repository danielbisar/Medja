namespace Medja.Utils
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Returns true if the first bytes of <paramref name="data"/> match <paramref name="pattern"/>.
        /// </summary>
        /// <returns>Returns <c>true</c> if the first bytes of <paramref name="data"/> match <paramref name="pattern"/> else <c>false</c>.</returns>
        /// <param name="data">Data.</param>
        /// <param name="pattern">Pattern.</param>
        public static bool StartsWith(this byte[] data, byte[] pattern)
        {
            if (data.Length < pattern.Length)
                return false;

            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] != data[i])
                    return false;
            }

            return true;
        }
    }
}