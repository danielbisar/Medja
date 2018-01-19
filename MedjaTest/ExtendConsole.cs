using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedjaTest
{
    /// <summary>
    /// Console related extensions
    /// </summary>
    public static class ExtendConsole
    {
        /// <summary>
        /// Prints the given strings on the console.
        /// </summary>
        /// <param name="lines">The lines to print.</param>
        public static void Print(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }
}
