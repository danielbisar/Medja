using System;
using System.IO;

namespace Medja.Utils.IO
{
    public class FileCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the full file path of the created file.
        /// </summary>
        public string FullFilePath { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileName">The filename, relative or absolute.</param>
        public FileCreatedEventArgs(string fileName)
        {
            FullFilePath = Path.GetFullPath(fileName);
        }
    }
}