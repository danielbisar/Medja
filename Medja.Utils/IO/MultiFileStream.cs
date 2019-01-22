using System;
using System.Collections.Generic;
using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Allows reading/writing big amounts of data and automatically handles file splitting (f.e. if the file system
    /// only allows files size up to 4GB)
    /// </summary>
    public class MultiFileStream : Stream
    {
        private readonly SequentialFileNames _sequentialFileNames;
        private readonly MultiFileStreamSettings _settings;

        // the currently used internal stream
        private Stream _currentStream;
        private bool _isDisposed;
        private int _fileNameIndex;

        public override bool CanRead
        {
            get { return _currentStream?.CanRead ?? false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _currentStream?.CanWrite ?? false; }
        }

        private long _length;
        /// <summary>
        /// Gets the length. Currently this only returns the sum of all files that existed when the
        /// <see cref="MultiFileStream"/> was created. TODO: implement correct behavior in class. 
        /// </summary>
        public override long Length
        {
            get { return _length; }
        }

        private long _position;
        /// <summary>
        /// Gets the position inside the stream(s). Seeking is currently not supported.
        /// </summary>
        public override long Position
        {
            get { return _position;}
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public MultiFileStream(MultiFileStreamSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _settings.Validate();

            _sequentialFileNames = new SequentialFileNames(settings.BaseName);
            _fileNameIndex = -1;

            PrepareFilesAndNames();
            
            OpenNextStream(true);
        }

        private void PrepareFilesAndNames()
        {
            _sequentialFileNames.Load();

            var fileMode = _settings.FileMode;
            
            if (fileMode == FileMode.Create)
            {
                foreach(var fileName in _sequentialFileNames.FileNames)
                    File.Delete(fileName);
                
                _sequentialFileNames.Clear();
            }
            else if (fileMode == FileMode.Open && _sequentialFileNames.FileNames.Count == 0)
            {
                throw new FileNotFoundException("Could not find any file matching the specified base file name.", _sequentialFileNames.BaseName);
            }
            else if(fileMode == FileMode.CreateNew && _sequentialFileNames.FileNames.Count != 0)
                throw new IOException("Files with the given baseFileName already exist.");

            foreach (var fileName in _sequentialFileNames.FileNames)
                _length += new FileInfo(fileName).Length;
            
            _fileNameIndex = -1;
        }

        private void OpenNextStream(bool allowNewFile)
        {
            _currentStream?.Dispose();

            _fileNameIndex++;

            string fileName;

            if (_fileNameIndex == _sequentialFileNames.FileNames.Count)
            {
                if(!allowNewFile || _settings.FileMode == FileMode.Open)
                    throw new EndOfStreamException();
                
                fileName = _sequentialFileNames.AddNext();
            }
            else
                fileName = _sequentialFileNames.FileNames[_fileNameIndex];
            
            _currentStream = OpenStream(fileName);
        }

        private Stream OpenStream(string fileName)
        {
            var fileAccess = _settings.FileAccess ??
                    (_settings.FileMode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite);
            
            return new FileStream(fileName, _settings.FileMode, fileAccess, FileShare.None, _settings.BufferSize, _settings.FileOptions);
        }

        /// <summary>
        /// Gets all currently used file names.
        /// </summary>
        /// <returns>An array of all used files names belonging to this stream.</returns>
        public IReadOnlyList<string> GetFileNames()
        {
            return _sequentialFileNames.FileNames;
        }

        public override void Flush()
        {
            _currentStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // both Position and Length are properties in FileStream that use calculations
            var pos = _currentStream.Position;
            var len = _currentStream.Length;
            int readBytes;
            
            if (pos + count <= len)
            {
                readBytes = _currentStream.Read(buffer, offset, count);
                _position += readBytes;
                
                return readBytes;
            }

            readBytes = (int) (len - pos);
            readBytes = _currentStream.Read(buffer, offset, readBytes);

            _position += readBytes;
            
            // no more files to read from left
            if (_fileNameIndex == _sequentialFileNames.FileNames.Count - 1)
                return readBytes;
            
            offset += readBytes;
            count -= readBytes;

            OpenNextStream(false);
                
            // call read to handle multiple overflows
            return readBytes + Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if(!CanSeek)
                throw new InvalidOperationException();
            
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // both Position and Length are properties in FileStream that use calculations
            var pos = _currentStream.Position;

            if (pos + count <= _settings.MaxFileSize)
            {
                _currentStream.Write(buffer, offset, count);
                _position += count;
            }
            else
            {
                var leftBytesForCurrentStream = (int)(_settings.MaxFileSize - pos);
                _currentStream.Write(buffer, offset, leftBytesForCurrentStream);

                _position += leftBytesForCurrentStream;
                offset += leftBytesForCurrentStream;
                count -= leftBytesForCurrentStream;

                OpenNextStream(true);
                
                // call write to handle multiple overflows (if count is > N times _maxFileSize)
                Write(buffer, offset, count);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                base.Dispose(disposing);

                if (_currentStream != null)
                {
                    _currentStream.Dispose();
                    _currentStream = null;
                }

                _isDisposed = true;
            }
        }
    }
}