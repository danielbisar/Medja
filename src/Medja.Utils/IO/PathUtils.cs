using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Some utility functions working a little different or not found in System.IO.Path
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Gets a directory path (absolute) from a file or folder path.
        /// </summary>
        /// <param name="path">A directory or folder path (relative or absolute).</param>
        /// <returns>The absolute directory path.</returns>
        /// <remarks>Different to GetDirectoryName in the sense that the result is never an empty string,
        /// even if you pass a relative path like "myfile.txt".</remarks>
        public static string GetDirectoryPath(string path)
        {
            var fullPath = Path.GetFullPath(path);
            return Path.GetDirectoryName(fullPath);
        }
    }
}