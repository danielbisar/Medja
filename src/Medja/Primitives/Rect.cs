namespace Medja.Primitives
{
    public class Rect
    {
        /// <summary>
        /// Substracts the given <see cref="Thickness"/> from the <see cref="Rect"/> and returns a new instance.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
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
        
        public bool IsWithin(Point point)
        {
            return point.X >= X
                    && point.Y >= Y
                    && point.X <= X + Width
                    && point.Y <= Y + Height;
        }

        public void Subtract(Thickness thickness)
        {
            X += thickness.Left;
            Y += thickness.Top;
            Width -= thickness.LeftAndRight;
            Height -= thickness.TopAndBottom;
        }
        
        /// <summary>
        /// Subtracts the given value from the current rect.
        /// </summary>
        /// <param name="value"></param>
        public void SubtractTop(float value)
        {
            Y += value;
            Height -= value;
        }

        /// <summary>
        /// Sets all values based on the given rect.
        /// </summary>
        /// <param name="rect">The rect with the "source" values.</param>
        public void SetFrom(Rect rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        public override string ToString()
        {
            return "X = " + X + ", Y = " + Y + ", Width = " + Width + ", Height = " + Height;
        }

        protected bool Equals(Rect other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Rect other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !Equals(left, right);
        }
    }
}
