using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    public class Thickness
    {
        public static readonly Thickness Empty = new Thickness();

        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        public Thickness()
            : this(0,0,0,0)
        {
        }

        public Thickness(float thickness)
            : this(thickness, thickness, thickness, thickness)
        {
        }

        public Thickness(float leftRight, float topBottom)
            : this(leftRight, topBottom, leftRight, topBottom)
        {
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
