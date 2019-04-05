using System.Collections.Generic;

namespace Medja.Utils.Collections.Generic
{
    /// <summary>
    /// It seems that <see cref="EqualityComparer{T}.Default"/> does more than just returning the value.
    /// So we cache it inside this class (Singleton, Generic -> for each type).
    /// Brings a little performance for Property object creation (Factor 1.26)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualityComparerCache<T>
    {
        // strange but a static getter is faster than static readonly in this case...
        public static EqualityComparer<T> Comparer { get; }

        static EqualityComparerCache()
        {
            Comparer = EqualityComparer<T>.Default;
        }
    }
}
