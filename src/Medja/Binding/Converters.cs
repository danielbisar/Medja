namespace Medja
{
    /// <summary>
    /// Provides some default converters for bindings.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Converts a string to <see cref="int"/>.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="defaultValue">The value that should be returned if parsing of the string failed.</param>
        /// <returns>The int value or <see cref="defaultValue"/> if parsing of the string failed.</returns>
        public static int ToInt(string value, int defaultValue = 0)
        {
            return int.TryParse(value, out var intVal) 
                ? intVal 
                : defaultValue;
        }

        /// <summary>
        /// Converts a string to <see cref="uint"/>.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="defaultValue">The value that should be returned if parsing of the string failed.</param>
        /// <returns>The uint value or <see cref="defaultValue"/> if parsing of the string failed.</returns>
        public static uint ToUint(string value, uint defaultValue = 0)
        {
            return uint.TryParse(value, out var uintVal) ? uintVal : defaultValue;
        }
    }
}
