using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    public class Size
    {
        public static readonly Size Empty = new Size(0, 0);

        public float Width { get; set; }
        public float Height { get; set; }

        public Size()
        {
        }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
