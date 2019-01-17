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

        /// <summary>
        /// Uses <see cref="BitConverter.ToInt32"/> if the segment has at least 4 bytes left.
        /// </summary>
        /// <param name="arraySegment">The <see cref="ArraySegment{Byte}"/>.</param>
        /// <returns>0 if less than 4 bytes are left, else the converter value.</returns>
        public static int ToInt32(this ArraySegment<byte> arraySegment)
        {
            if (arraySegment.Count < 4)
                return 0;

            return BitConverter.ToInt32(arraySegment.Array, arraySegment.Offset);
        }
    }
}