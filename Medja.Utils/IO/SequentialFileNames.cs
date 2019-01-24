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

        /// <summary>
        /// Gets the <see cref="BaseName"/>. This is the suffix of the file names generated including the path
        /// (can be relative).
        /// </summary>
        public string BaseName
        {
            get { return _baseName; }
        }

        private readonly List<string> _fileNames;
        public IReadOnlyList<string> FileNames
        {
            get { return _fileNames; }
        }
        
        public SequentialFileNames(string baseName)
        {
            if(string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentException("May not be null or empty or whitespaces.",nameof(baseName));
            
            _baseName = baseName;
            _fileNames = new List<string>();
        }

        /// <summary>
        /// Adds the next file name.
        /// </summary>
        /// <returns>The added file name.</returns>
        public string AddNext()
        {
            // it seems .net core has a bug that will lead to new FileInfo(_baseName).FullName if the '.' is 
            // included in _baseName FullName would omit it.
            var fileName = new FileInfo(_baseName).FullName + "." + _fileNames.Count;
            _fileNames.Add(fileName);

            return fileName;
        }

        public void Load()
        {
            Clear();

            var baseFileInfo = new FileInfo(_baseName + ".");
            var files = baseFileInfo.Directory.GetFiles(baseFileInfo.Name + "*");
            var numbers = new List<int>(files.Length);

            foreach (var fileInfo in files)
            {
                var name = fileInfo.Name;
                var lastIndexOfDot = name.LastIndexOf('.');
                
                if(lastIndexOfDot == -1)
                    continue;
                
                // ignore other files names (f.e. baseName = 'abc.' and found file is 'abc..0')
                if(baseFileInfo.Name.Length-1 != lastIndexOfDot)
                    continue;
                
                if(!int.TryParse(name.Substring(lastIndexOfDot + 1), out var n))
                    continue;

                _fileNames.Add(fileInfo.FullName);
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