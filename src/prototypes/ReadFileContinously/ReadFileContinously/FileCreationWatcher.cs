using System;
using System.IO;

namespace Medja.Utils.IO
{
    public class FileCreationWatcher
    {
        private readonly string _fullFileName;
        private readonly FileSystemWatcher _watcher;
        
        /// <summary>
        /// Raised when the file is created.
        /// </summary>
        public event EventHandler<FileCreatedEventArgs> Created;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileName">The filename of the file that is going to be created (relative or absolute).</param>
        public FileCreationWatcher(string fileName)
        {
            _fullFileName = Path.GetFullPath(fileName);
            _watcher = new FileSystemWatcher()
        }
        
        
    }
}