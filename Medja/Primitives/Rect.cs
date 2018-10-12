namespace Medja.Primitives
{
    public class Rect
    {
        public static Rect Subtract(Rect rect, Thickness margin)
        {
            return new Rect
            {
                X = rect.X - margin.Left,
                Y = rect.Y - margin.Top,
                Width = rect.Width - margin.LeftAndRight,
                Height = rect.Height - margin.TopAndBottom
            };
        }
        
        public virtual float X { get; set; }
        public virtual float Y { get; set; }
        public virtual float Width { get; set; }
        public virtual float Height { get; set; }

        public Rect()
        {
        }

        public Rect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect(Rect position)
        {
            X = position.X;
            Y = position.Y;
            Width = position.Width;
            Height = position.Height;
        }

        public void MoveTo(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
        
        public bool IsWithin(Point point)
        {
            return point.X >= X
                    && point.Y >= Y
                    && point.X <= X + Width
                    && point.Y <= Y + Height;
        }

        public void Subtract(Thickness thickness)
        {
            X -= thickness.Left;
            Y -= thickness.Top;
            Width -= thickness.LeftAndRight;
            Height -= thickness.TopAndBottom;
        }

        public override string ToString()
        {
            return "X = " + X + ", Y = " + Y + ", Width = " + Width + ", Height = " + Height;
        }

        /// <summary>
        /// Subtracts the given value from the current rect.
        /// </summary>
        /// <param name="value"></param>
        public void SubtractTop(float value)
        {
            Y -= value;
            Height -= value;
        }
    }
}
