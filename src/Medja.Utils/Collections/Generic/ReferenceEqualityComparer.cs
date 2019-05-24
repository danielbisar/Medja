using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Medja.Utils.Collections.Generic
{
    /// <summary>
    /// An equality comparer that uses the identity of an object instead of it's values to create a hash code and
    /// ReferenceEquals for the Equals method. 
    /// </summary>
    /// <typeparam name="T">The objects type. Restricted to reference types.</typeparam>
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T: class
    {
        /// <summary>
        /// Returns true if ReferenceEquals returns true.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary>
        /// Gets a hash code based on identity. <see cref="RuntimeHelpers.GetHashCode(object)"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}