using Medja.Properties;

namespace Medja.Primitives
{
    public class Thickness
    {
        public readonly Property<float> PropertyTop;

        public float Top
        {
            get { return PropertyTop.Get(); }
            set { PropertyTop.Set(value); }
        }

        public readonly Property<float> PropertyBottom;

        public float Bottom
        {
            get { return PropertyBottom.Get(); }
            set { PropertyBottom.Set(value); }
        }

        public readonly Property<float> PropertyLeft;

        public float Left
        {
            get { return PropertyLeft.Get(); }
            set { PropertyLeft.Set(value); }
        }

        public readonly Property<float> PropertyRight;

        public float Right
        {
            get { return PropertyRight.Get(); }
            set { PropertyRight.Set(value); }
        }
        
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
            PropertyBottom = new Property<float>();
            PropertyTop = new Property<float>();
            PropertyLeft = new Property<float>();
            PropertyRight = new Property<float>();
            
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

        public void SetTopAndBottom(float value)
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
