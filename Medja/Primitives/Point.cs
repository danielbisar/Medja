using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point()
            : this(0, 0)
        {
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
