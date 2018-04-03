using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Medja.Primitives
{
    [DebuggerDisplay("Width = {Width}, Height = {Height}")]
    public class Size
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Size()
            : this(0, 0)
        {
        }

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return "Width = " + Width + ", Height = " + Height;
        }
    }
}
