namespace Medja.Primitives
{
    public class Thickness
    {
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        // TODO check performance implications of the calculation inside the getter (verify overall performance)
        
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

        public void SetLeftAndRight(float value)
        {
            Left = Right = value;
        }

        public void SetTopAndBotton(float value)
        {
            Top = Bottom = value;
        }

        public void SetFrom(Thickness thickness)
        {
            Left = thickness.Left;
            Top = thickness.Top;
            Right = thickness.Right;
            Bottom = thickness.Bottom;
        }

        public override string ToString()
        {
            return $"Left = {Left}, Top = {Top}, Right = {Right}, Bottom = {Bottom}";
        }
    }
}
