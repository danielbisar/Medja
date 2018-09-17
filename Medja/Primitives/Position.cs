namespace Medja.Primitives
{
    public class Position
    {
        public virtual float X { get; set; }
        public virtual float Y { get; set; }
        public virtual float Width { get; set; }
        public virtual float Height { get; set; }

        public Position()
        {
        }

        public Position(Position position)
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

        public override string ToString()
        {
            return "X = " + X + ", Y = " + Y + ", Width = " + Width + ", Height = " + Height;
        }
    }
}
