using System;

namespace Medja.Primitives
{
    public class Thickness
    {
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        public float TopAndBottom { get { return Top + Bottom; } }
        public float LeftAndRight { get { return Left + Right; } }

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

        public void SetAll(float value)
        {
            Top = Bottom = Left = Right = value;
        }

        public override string ToString()
        {
            return $"Left = {Left}, Top = {Top}, Right = {Right}, Bottom = {Bottom}";
        }
    }
}
