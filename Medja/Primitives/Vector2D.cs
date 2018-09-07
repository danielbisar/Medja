﻿namespace Medja.Primitives
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
        /// Initializes the vector using two points. Point one is the origin.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Vector2D(Point p1, Point p2)
        {
            X = p2.X - p1.X;
            Y = p2.Y - p1.Y;
        }

        public override string ToString()
        {
            return string.Format("Vector2D: {0},{1}", X, Y);
        }
    }
}
