using System;
using System.Collections.Concurrent;

namespace Medja.Utils.Collections.Concurrent
{
    public static class ConcurrentQueueExtensions
    {
        /// <summary>
        /// Calls TryDequeue as long as it returns true and executes dequeueAction per item.
        /// </summary>
        /// <param name="queue"></param>
        /// <typeparam name="T"></typeparam>
        public static void TryDequeueAll<T>(this ConcurrentQueue<T> queue, Action<T> dequeueAction)
        {
            while (queue.TryDequeue(out var result))
            {
                dequeueAction(result);
            }
        }
    }
}