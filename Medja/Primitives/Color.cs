using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Primitives
{
    public class Color
    {
        public float Red { get; }
        public float Green { get; }
        public float Blue { get; }

        public Color(float red, float green, float blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public override string ToString()
        {
            return "Red = " + Red + ", Green = " + Green + ", Blue = " + Blue;
        }
    }
}
