using System;
using System.IO;

namespace ReadFileContinously
{
    /// <summary>
    /// Follows updates of a file.
    /// </summary>
    public class FileFollowReader : IDisposable
    {
        private readonly FileSystemWatcher _watcher;
        private readonly StreamReader _reader;
        private readonly string _fullFileName;

        /// <summary>
        /// Raised for every file change and once initially after calling <see cref="Start"/>.
        /// </summary>
        public event EventHandler<ReadEventArgs> DataRead;

        public FileFollowReader(string fileName)
        {
            _fullFileName = Path.GetFullPath(fileName);
            _reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            
            _watcher = new FileSystemWatcher(GetFolder(fileName));
            _watcher.NotifyFilter = NotifyFilters.Size;
            _watcher.Changed += OnWatcherChanged;
        }

        /// <summary>
        /// Start watching the file - raises initial changed event so that no content is lost.
        /// </summary>
        public void Start()
        {
            RaiseDataRead();
            _watcher.EnableRaisingEvents = true;
        }

        private void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            // e.FullPath does NOT contain the whole path!
            var fullPath = Path.GetFullPath(e.FullPath);

            // just check the current file, other events are ignored
            if (_fullFileName != fullPath)
                return;

            if (e.ChangeType == WatcherChangeTypes.Changed)
                RaiseDataRead();
        }

        private void RaiseDataRead()
        {
            var data = _reader.ReadToEnd();
            DataRead?.Invoke(this, new ReadEventArgs(data));
        }

        public void Dispose()
        {
            _watcher.Dispose();
            _reader.Dispose();
        }
    }
}