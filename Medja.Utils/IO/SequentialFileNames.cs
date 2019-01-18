using System;
using System.Collections.Generic;
using System.IO;

namespace Medja.Utils.IO
{
    /// <summary>
    /// This class handles files names with suffix .0 until .N - it can verify the pattern, load it from exiting files
    /// and you can add or remove a file name.
    /// </summary>
    public class SequentialFileNames
    {
        private readonly string _baseName;
        private readonly List<string> _fileNames;
        public IReadOnlyList<string> FileNames
        {
            get { return _fileNames; }
        }
        
        public SequentialFileNames(string baseName)
        {
            if(string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentException("May not be null or empty or whitespaces.",nameof(baseName));
            
            _baseName = baseName + ".";
            _fileNames = new List<string>();
        }

        /// <summary>
        /// Adds the next file name.
        /// </summary>
        /// <returns>The added file name.</returns>
        public string AddNext()
        {
            var fileName = _baseName + _fileNames.Count;
            _fileNames.Add(fileName);

            return fileName;
        }

        public void Load()
        {
            Clear();

            var files = new FileInfo(_baseName).Directory.GetFiles(_baseName + "*");
            var numbers = new List<int>(files.Length);

            foreach (var fileInfo in files)
            {
                var name = fileInfo.Name;
                var lastIndexOfDot = name.LastIndexOf('.');
                
                if(lastIndexOfDot == -1)
                    continue;
                
                if(!int.TryParse(name.Substring(lastIndexOfDot + 1), out var n))
                    continue;

                _fileNames.Add(name);
                numbers.Add(n);
            }
            
            numbers.Sort();
            _fileNames.Sort();

            for (int i = 0; i < numbers.Count; i++)
            {
                if(numbers[i] != i)
                    throw new InvalidOperationException($"The base name you use must be a name where either no file exists with the same name + .N where N is an integer number or all files that exist in the sequence from 0 until the highest.");
            }
        }

        public void Clear()
        {
            _fileNames.Clear();
            //_fileNames.TrimExcess();
        }
    }
}