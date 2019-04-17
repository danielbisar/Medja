using System;
using System.Collections.Generic;
using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// Handles multiple sequential files.
    /// </summary>
    public class MultiFileManager
    {
        private readonly SequentialFileNames _sequentialFileNames;
        private readonly List<long> _fileSizes;
        
        private int _currentFileIndex;

        private long _length;
        public long Length
        {
            get{ return _length;}
        }

        public int FileCount
        {
            get { return _fileSizes.Count; }
        }

        public string BaseName
        {
            get { return _sequentialFileNames.BaseName; }
        }

        public IReadOnlyList<string> FileNames
        {
            get { return _sequentialFileNames.FileNames; }
        }

        public MultiFileManager(string baseName)
        {
            _fileSizes = new List<long>();
            _sequentialFileNames = new SequentialFileNames(baseName);
            _currentFileIndex = -1;
        }

        public void Load()
        {
            _sequentialFileNames.Load();
            ReadFileSizes();
            _currentFileIndex = -1;
        }
        
        private void ReadFileSizes()
        {
            foreach (var fileName in _sequentialFileNames.FileNames)
            {
                var length = new FileInfo(fileName).Length; 
                _length += length;
                _fileSizes.Add(length);
            }
        }

        public void DeleteAll()
        {
            foreach(var fileName in _sequentialFileNames.FileNames)
                File.Delete(fileName);
        }

        public void SetCurrentFileLength(long position)
        {
            _fileSizes[_currentFileIndex] = position;
        }

        public long SetFileAtPos(long position)
        {
            long bytes = 0;

            for (_currentFileIndex = 0; _currentFileIndex < _fileSizes.Count; _currentFileIndex++)
            {
                var sum = bytes + _fileSizes[_currentFileIndex];

                if (sum < position)
                    bytes = sum;
                else
                    return position - bytes;
            }

            return -1;
        }

        public string GetNext()
        {
            _currentFileIndex++;
            return _sequentialFileNames.FileNames[_currentFileIndex];
        }

        public string CreateNext()
        {
            _currentFileIndex++;
            _fileSizes.Add(0);
            
            if(_currentFileIndex >= _fileSizes.Count)
                throw new InvalidOperationException();
            
            return _sequentialFileNames.AddNext();
        }

        public bool HasNext()
        {
            return _currentFileIndex + 1 < _fileSizes.Count;
        }

        public string GetCurrentFileName()
        {
            return _sequentialFileNames.FileNames[_currentFileIndex];
        }
    }
}