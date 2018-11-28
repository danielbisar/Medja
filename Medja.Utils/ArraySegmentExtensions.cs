using System;

namespace Medja.Utils
{
    /// <summary>
    /// Extensions for <see cref="ArraySegment{T}"/>
    /// </summary>
    public static class ArraySegmentExtensions
    {
        /// <summary>
        /// Gets whether the ArraySegment is empty or not.
        /// </summary>
        /// <param name="arraySegment">The <see cref="ArraySegment{T}"/>.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>true if the <see cref="ArraySegment{T}.Array"/> is null or <see cref="ArraySegment{T}.Count"/>
        /// is 0.</returns>
        public static bool IsEmpty<T>(this ArraySegment<T> arraySegment)
        {
            return arraySegment.Array == null || arraySegment.Count == 0;
        }
    }
}