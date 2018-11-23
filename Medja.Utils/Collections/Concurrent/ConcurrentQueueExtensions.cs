using System.Collections.Concurrent;

namespace Medja.Utils.Collections.Concurrent
{
    public static class ConcurrentQueueExtensions
    {
        /// <summary>
        /// Clears all items in the queue. NOTE: does not necessarily result in an empty queue but can be helpful
        /// if you want to remove (almost) all elements. If you can assure no enqueue is called during or after the
        /// call of Clear the queue will be empty.
        /// </summary>
        /// <param name="queue"></param>
        /// <typeparam name="T"></typeparam>
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out var result))
            {
            }
        }
    }
}