using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Primitives
{
    public class Size
    {
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
