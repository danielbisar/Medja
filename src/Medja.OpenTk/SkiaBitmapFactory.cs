using System;
using Medja.Controls.Images;
using SkiaSharp;

namespace Medja.OpenTk
{
    /// <summary>
    /// Using skia to load bitmaps.
    /// </summary>
    public class SkiaBitmapFactory : BitmapFactory
    {
        private int _nextId;

        public SkiaBitmapFactory()
        {
            _nextId = 0;
        }
        
        protected override Bitmap Load(string fileName)
        {
            var targetImageInfo = SKBitmap.DecodeBounds(fileName);

            // BUGFIX: for SkiaSharp 1.68.0 => when calling SKBitmap.DecodeBounds for the first time, it will not work
            if (targetImageInfo.IsEmpty)
                targetImageInfo = SKBitmap.DecodeBounds(fileName);
            
            if(targetImageInfo.IsEmpty)
                throw new Exception("Could not load the image.");
            
            //targetImageInfo.ColorType = SKColorType.Rgba8888;
            
            var skBitmap = SKBitmap.Decode(fileName, targetImageInfo);
            var bitmap = new Bitmap(_nextId++, fileName, skBitmap.Bytes, PixelFormat.RGBA32, skBitmap.Width, skBitmap.Height);
            bitmap.BackendBitmap = skBitmap;
            
            //skBitmap.Dispose();

            return bitmap;
        }
    }
}