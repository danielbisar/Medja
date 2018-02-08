using System;
using System.Collections.Generic;
using System.Text;

namespace Medja
{
    /// <summary>
    /// It seems that EqualityComparer<T>.Default does more than just returning the value.
    /// So we cache it inside this class (Singleton, Generic -> for each type).
    /// Brings a little performance for Property object creation (Factor 1.26)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EqualityComparerCache<T>
    {
        // strange but a Static getter is faster than static readonly in this case...
        public static EqualityComparer<T> Comparer { get; }

        static EqualityComparerCache()
        {
            Comparer = EqualityComparer<T>.Default;
        }
    }
}
