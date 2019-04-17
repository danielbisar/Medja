namespace Medja.Primitives
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

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}";
        }
    }
}
