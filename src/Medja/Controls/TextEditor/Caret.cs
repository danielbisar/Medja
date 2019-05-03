namespace Medja.Controls
{
    public class Caret
    {
        public static bool operator >(Caret a, Caret b)
        {
            if (a == null)
                return false;

            if (b == null)
                return true;
            
            if (a.Y == b.Y)
                return a.X > a.Y;

            return a.Y > b.Y;
        }

        public static bool operator <(Caret a, Caret b)
        {
            if (a == null)
                return true;

            if (b == null)
                return false;
            
            if (a.Y == b.Y)
                return a.X < b.X;

            return a.Y < b.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Caret()
            : this(0, 0)
        {
        }

        public Caret(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}