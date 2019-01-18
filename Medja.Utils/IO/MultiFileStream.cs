using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Allows reading/writing big amounts of data and automatically handles file splitting (f.e. if the file system
    /// only allows files size up to 4GB)
    /// </summary>
    public class MultiFileStream : Stream
    {
        private readonly FileMode _fileMode;
        private readonly long _maxFileSize;
        private readonly SequentialFileNames _sequentialFileNames;

        // the currently used internal stream
        private Stream _currentStream;
        private bool _isDisposed;
        private int _fileNameIndex;
        
        public override bool CanRead { get; }

        public override bool CanSeek
        {
            get { return _currentStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _currentStream.CanWrite; }
        }
        
        public override long Length { get; }
        public override long Position { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="baseFileName">The base filename. Each file gets a suffix starting from .0 until .N</param>
        /// <param name="fileMode">The <see cref="FileMode"/>.</param>
        /// <param name="maxFileSize">The maximum size of a single file. Is ignored for <see cref="FileMode.Open"/>.</param>
        public MultiFileStream(string baseFileName, FileMode fileMode, long maxFileSize)
        {
            if (fileMode != FileMode.OpenOrCreate && fileMode != FileMode.Open)
                throw new ArgumentOutOfRangeException(nameof(fileMode), fileMode, "The given mode is not supported.");
            
            if(fileMode != FileMode.Open && maxFileSize < 1)
                throw new ArgumentOutOfRangeException(nameof(maxFileSize), maxFileSize, "Must be >= 1 except for FileMode.Open.");

            _fileMode = fileMode;
            _maxFileSize = maxFileSize;
            _sequentialFileNames = new SequentialFileNames(baseFileName);
            _fileNameIndex = -1;

            VerifyExistingFiles();
            
            OpenNextStream(true);
        }

        private void VerifyExistingFiles()
        {
            _sequentialFileNames.Load();
            _fileNameIndex = -1;
        }

        private void OpenNextStream(bool allowNewFile)
        {
            _currentStream?.Dispose();

            _fileNameIndex++;

            string fileName;

            if (_fileNameIndex == _sequentialFileNames.FileNames.Count)
            {
                if(!allowNewFile || _fileMode == FileMode.Open)
                    throw new EndOfStreamException();
                
                fileName = _sequentialFileNames.AddNext();
            }
            else
                fileName = _sequentialFileNames.FileNames[_fileNameIndex];
            
            _currentStream = OpenStream(fileName);
        }

        private Stream OpenStream(string fileName)
        {
            return File.Open(fileName, _fileMode);
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
            
            if (pos + count <= len)
                return _currentStream.Read(buffer, offset, count);
            
            var readBytes = (int) (len - pos);
            readBytes = _currentStream.Read(buffer, offset, readBytes);

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
            
            if(pos + count <= _maxFileSize)
                _currentStream.Write(buffer, offset, count);
            else
            {
                var leftBytesForCurrentStream = (int)(_maxFileSize - pos);
                _currentStream.Write(buffer, offset, leftBytesForCurrentStream);

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
                    _currentStream.Dispose();
                
                _isDisposed = true;
            }
        }
    }
}