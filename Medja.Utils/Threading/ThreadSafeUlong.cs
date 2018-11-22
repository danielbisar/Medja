using System.Threading;

namespace Medja.Utils.Threading
{
    /// <summary>
    /// Uses Interlocked but allows usage of ulong.
    /// </summary>
    /// <remarks>Idea is based on: https://stackoverflow.com/questions/934619/c-sharp-multi-threaded-unsigned-increment
    /// (John Skeets answer).</remarks>
    public class ThreadSafeUlong
    {
        private long _value;

        /// <summary>
        /// Creates a new instance with the given value.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public ThreadSafeUlong(ulong value = 0)
        {
            _value = unchecked((long)value);
        }

        /// <summary>
        /// Increments the current value as atomic operation.
        /// </summary>
        /// <returns>The incremented value.</returns>
        public ulong Increment()
        {
            var result = Interlocked.Increment(ref _value);
            return unchecked((ulong) result);
        }

        /// <summary>
        /// Adds the given value as atomic operation.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>The result.</returns>
        public ulong Add(ulong value)
        {
            var result = Interlocked.Add(ref _value, unchecked((long) value));
            return unchecked((ulong) result);
        }

        /// <summary>
        /// Reads the current value as atomic operation.
        /// </summary>
        /// <returns>The current value.</returns>
        public ulong Read()
        {
            var result = Interlocked.Read(ref _value);
            return unchecked((ulong) result);
        }
        
        // TODO support the other Interlocked methods
    }
}