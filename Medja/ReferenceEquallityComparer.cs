using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Medja
{
    /// <summary>
    /// An equality comparer that uses the identity of an object instead of it's values to create a hash code and
    /// ReferenceEquals for the Equals method. 
    /// </summary>
    /// <typeparam name="T">The objects type. Restricted to reference types.</typeparam>
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T: class
    {
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}