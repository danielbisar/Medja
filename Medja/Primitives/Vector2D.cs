using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medja.Primitives
{
    public class Vector2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2D()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Initilizes the vector using two points. Point one is the start and point to the target;
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Vector2D(Point p1, Point p2)
        {
            X = p2.X - p1.X;
            Y = p2.Y - p1.Y;
        }
    }
}
