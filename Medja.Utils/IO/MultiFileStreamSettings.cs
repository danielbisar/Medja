using System;
using System.IO;

namespace Medja.Utils.IO
{
    public class MultiFileStreamSettings
    {
        /// <summary>
        /// Gets or sets the base name used for the actual file names. This includes the file path but can be relative.
        /// The suffix .N where N is the N-th file (starting at 0) will be append. Examples are
        /// ./baseName (=> ./baseName.0, ./baseName.1, ...) or /tmp/myFile.txt
        /// (=> /tmp/myFileName.txt.0, /tmp/myFileName.txt.1, ...).
        /// </summary>
        public string BaseName { get; set; }
        
        /// <summary>
        /// The <see cref="FileMode"/> that is used. <see cref="System.IO.FileMode.Append"/> and
        /// <see cref="System.IO.FileMode.Truncate"/>  are currently not supported.
        /// (default = <see cref="System.IO.FileMode.OpenOrCreate"/>).
        /// </summary>
        public FileMode FileMode { get; set; }
        
        /// <summary>
        /// The maximum size per File. If the size grows larger a new file will be created (depending on the FileMode,
        /// default = 1000000000 bytes = 1GB). Must be any value >= 1 for
        /// FileModes != <see cref="System.IO.FileMode.Open"/>.
        /// </summary>
        public long MaxFileSize { get; set; }
        
        /// <summary>
        /// The read/write buffer size in bytes used by the internal FileStream. (default = 8192).
        /// </summary>
        public int BufferSize { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="System.IO.FileAccess"/>. If not set it is set depending on the
        /// <see cref="FileMode"/>.
        /// </summary>
        public FileAccess? FileAccess { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="System.IO.FileOptions"/>.
        /// </summary>
        public FileOptions FileOptions { get; set; }

        public MultiFileStreamSettings()
        {
            BufferSize = 8192;
            MaxFileSize = 1000000000;
            FileMode = FileMode.OpenOrCreate;
            FileOptions = FileOptions.None;
        }

        internal void Validate()
        {
            if(FileMode == FileMode.Append || FileMode == FileMode.Truncate)
                throw new NotSupportedException($"The given {nameof(FileMode)} is currently not supported.");
            
            if(FileMode != FileMode.Open && MaxFileSize < 1)
                throw new ArgumentOutOfRangeException(nameof(MaxFileSize), MaxFileSize, "Must be >= 1 except for FileMode.Open.");
        }
    }
}