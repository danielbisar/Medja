using System;
using System.Collections.Generic;
using System.IO;
using Medja.Utils.Collections.Generic;

namespace Medja.Controls.Images
{
    public abstract class BitmapFactory
    {
        // TODO ways to clean the cache
        private readonly Dictionary<string, Bitmap> _bitmaps;
        
        protected BitmapFactory()
        {            
            _bitmaps = new Dictionary<string, Bitmap>();
        }

        /// <summary>
        /// Gets a bitmap from the cache or loads it.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The Bitmap object.</returns>
        /// <exception cref="NotSupportedException">See <see cref="Load"/>.</exception>
        public Bitmap Get(string fileName)
        {
            var fullFileName = new FileInfo(fileName).FullName;
            return _bitmaps.GetOrAdd(fullFileName, Load);
        }

        /// <summary>
        /// Loads a bitmap from a file (can be any format that is supported by the backend).
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The loaded bitmap.</returns>
        /// <exception cref="NotSupportedException">Is thrown if the file format is not supported.</exception>
        protected abstract Bitmap Load(string fileName);
    }
}