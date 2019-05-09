using System;

namespace Medja.Controls
{
    public class Caret : IEquatable<Caret>
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

        public static bool operator ==(Caret a, Caret b)
        {
            if (object.ReferenceEquals(a, null))
            {
                if (object.ReferenceEquals(b, null))
                    return true;

                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Caret a, Caret b)
        {
            return !(a == b);
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

        public bool Equals(Caret other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            
            if (obj.GetType() != this.GetType()) 
                return false;
            
            return Equals((Caret) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}