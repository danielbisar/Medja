using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Rendering
{
    public class Bitmap
    {
        public byte[] Data { get; }
        // TODO pixelformat
        public int Width { get; }
        public int Height { get; }

        public Bitmap(byte[] data, int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }
    }
}
