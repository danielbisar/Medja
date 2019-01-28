using System.IO;

namespace Medja.Controls.Images
{
    public class Bitmap
    {
        /// <summary>
        /// A unique id.
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// The full filename (if any).
        /// </summary>
        public string FullFileName { get; }
        
        /// <summary>
        /// The bitmap data as byte array.
        /// </summary>
        public byte[] Data { get; }
        
        /// <summary>
        /// The pixel format.
        /// </summary>
        public PixelFormat PixelFormat { get; }
        
        public int Width { get; }
        public int Height { get; }
        
        /// <summary>
        /// Gets the object that can be used by the rendering backend to render the bitmap.
        /// </summary>
        public object BackendBitmap { get; set; }

        public Bitmap(int id, string fileName, byte[] data, PixelFormat pixelFormat, int width, int height)
        {
            Id = id;
            Data = data;
            PixelFormat = pixelFormat;
            Width = width;
            Height = height;

            if(!string.IsNullOrWhiteSpace(fileName))
                FullFileName = new FileInfo(fileName).FullName;
        }
    }
}