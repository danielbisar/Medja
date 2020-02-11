using System;
using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Watches the file system for a file to be created with a specific path.
    /// </summary>
    public class FileCreatedWatcher : IDisposable
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
        public FileCreatedWatcher(string fileName)
        {
            _fullFileName = Path.GetFullPath(fileName);
            _watcher = new FileSystemWatcher(PathUtils.GetDirectoryPath(fileName));
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName;
            _watcher.Created += OnFileCreated;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // e.FullPath does NOT contain the whole path!
            var fullPath = Path.GetFullPath(e.FullPath);

            // just check the current file, other events are ignored
            if (_fullFileName != fullPath)
                return;
            
            RaiseCreated();
        }

        private void RaiseCreated()
        {
            Created?.Invoke(this, new FileCreatedEventArgs(_fullFileName));
        }

        /// <summary>
        /// Starts watching.
        /// </summary>
        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}