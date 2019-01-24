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
        private readonly MultiFileStreamSettings _settings;
        private readonly MultiFileManager _multiFileManager;
        private string _currentFileName;
        
        // the currently used internal stream
        private Stream _currentStream;
        private bool _isDisposed;

        public override bool CanRead
        {
            get { return _currentStream?.CanRead ?? false; }
        }

        public override bool CanSeek
        {
            get { return _currentStream != null; }
        }

        public override bool CanWrite
        {
            get { return _currentStream?.CanWrite ?? false; }
        }

        /// <summary>
        /// Gets the length. Currently this only returns the sum of all files that existed when the
        /// <see cref="MultiFileStream"/> was created. TODO: implement correct behavior in class. 
        /// </summary>
        public override long Length
        {
            get { return _multiFileManager.Length; }
        }

        private long _position;
        /// <summary>
        /// Gets the position inside the stream(s). Seeking is currently not supported.
        /// </summary>
        public override long Position
        {
            get { return _position;}
            set { Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public MultiFileStream(MultiFileStreamSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _settings.Validate();

            _multiFileManager = new MultiFileManager(_settings.BaseName);

            PrepareFilesAndNames();
            
            OpenNextStream(true);
        }

        private void PrepareFilesAndNames()
        {
            _multiFileManager.Load();

            var fileMode = _settings.FileMode;
            
            if (fileMode == FileMode.Create)
                _multiFileManager.DeleteAll();
            else if (fileMode == FileMode.Open && _multiFileManager.FileCount == 0)
                throw new FileNotFoundException("Could not find any file matching the specified base file name.", _multiFileManager.BaseName);
            else if(fileMode == FileMode.CreateNew && _multiFileManager.FileCount != 0)
                throw new IOException("Files with the given BaseName already exist.");
        }

        private void OpenNextStream(bool allowNewFile)
        {
            _currentStream?.Dispose();

            string fileName;

            if (!_multiFileManager.HasNext())
            {
                if (!allowNewFile || _settings.FileMode == FileMode.Open)
                    throw new EndOfStreamException();

                fileName = _multiFileManager.CreateNext();
            }
            else
                fileName = _multiFileManager.GetNext();
            
            _currentStream = OpenStream(fileName);
        }

        private Stream OpenStream(string fileName)
        {
            var fileAccess = _settings.FileAccess ??
                    (_settings.FileMode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite);
            _currentFileName = fileName;
            
            return new FileStream(fileName, _settings.FileMode, fileAccess, FileShare.None, _settings.BufferSize, _settings.FileOptions);
        }

        /// <summary>
        /// Gets all currently used file names.
        /// </summary>
        /// <returns>An array of all used files names belonging to this stream.</returns>
        public IReadOnlyList<string> GetFileNames()
        {
            return _multiFileManager.FileNames;
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
            if (!_multiFileManager.HasNext())
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

            long newPosition;

            if (origin == SeekOrigin.Begin)
                newPosition = offset;
            else if (origin == SeekOrigin.Current)
                newPosition = _position + offset;
            else // if(origin == SeekOrigin.End)
                newPosition = Length + offset;
            
            if(newPosition >= Length || newPosition < 0)
                throw new IOException();

            var relativeBytes = _multiFileManager.SetFileAtPos(newPosition);
            
            if(relativeBytes == -1)
                throw new IOException();
            
            _position = newPosition;

            var fileName = _multiFileManager.GetCurrentFileName();

            if (fileName != _currentFileName)
            {
                _currentStream?.Dispose();
                _currentStream = OpenStream(fileName);
            }
            
            _currentStream.Position = relativeBytes;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // both Position and Length are properties in FileStream that use calculations
            var streamPos = _currentStream.Position;

            if (streamPos + count <= _settings.MaxFileSize)
            {
                _currentStream.Write(buffer, offset, count);
                _multiFileManager.SetCurrentFileLength(_currentStream.Length);
                _position += count;
            }
            else
            {
                var leftBytesForCurrentStream = (int)(_settings.MaxFileSize - streamPos);
                _currentStream.Write(buffer, offset, leftBytesForCurrentStream);
                _multiFileManager.SetCurrentFileLength(_currentStream.Length);
                
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