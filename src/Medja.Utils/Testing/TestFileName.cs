using System;
using System.IO;

namespace Medja.Utils.Testing
{
    public static class TestFileName
    {
        /// <summary>
        /// Gets a random filename. 
        /// </summary>
        /// <param name="numberFormat">The number format to use (internally an integer generated and with ToString(...)
        /// converted into a string.</param>
        /// <returns>The random file name.</returns>
        public static string GetRandomFileName(string numberFormat)
        {
            var random = new Random();
            var number = random.Next() + 1;

            return number.ToString(numberFormat);
        }
        
        /// <summary>
        /// Gets a random file name and makes sure there is no file with that name.
        /// </summary>
        /// <returns>The random file name.</returns>
        public static string GetNotExisting()
        {
            var fileName = GetRandomFileName("X");

            while (File.Exists(fileName))
            {
                fileName = GetRandomFileName("X");
            }

            return fileName;
        }
    }
}