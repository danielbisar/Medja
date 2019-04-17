using System;
using System.Threading.Tasks;

namespace Medja.Utils.Threading.Tasks
{
    public static class TaskQueueExtensions
    {
        /// <summary>
        /// Enqueues the given function on the queue.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <typeparam name="TResult">The expected task result type.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task<TResult> Enqueue<TResult>(this TaskQueue<TResult> taskQueue, Func<TResult> func)
        {
            return taskQueue.Enqueue(state => func(), null);
        }

        /// <summary>
        /// Enqueues the given action on the queue.
        /// </summary>
        /// <param name="taskQueue">The <see cref="TaskQueue{TResult}"/>.</param>
        /// <param name="action">The action to execute.</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task Enqueue<TResult>(this TaskQueue<TResult> taskQueue, Action action)
        {
            return taskQueue.Enqueue(state =>
            {
                action();
                return default;
            }, null);
        }

        /// <summary>
        /// Enqueues the given function onto the queue.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <typeparam name="TResult">The expected task result type.</typeparam>
        /// <typeparam name="TParam">The functions parameter type (the state objects type).</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task<TResult> Enqueue<TResult, TParam>(this TaskQueue<TResult> taskQueue,
                                                             Func<TParam, TResult> func,
                                                             TParam param)
        {
            return taskQueue.Enqueue(state => func((TParam) state), param);
        }

        /// <summary>
        /// Enqueues the given action on the queue. 
        /// </summary>
        /// <param name="taskQueue">The <see cref="TaskQueue{TResult}"/>.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="param">The state object (any param you want to pass).</param>
        /// <typeparam name="TResult">The <see cref="TaskQueue{TResult}"/>s result type.</typeparam>
        /// <typeparam name="TParam">The actions parameter type.</typeparam>
        /// <returns>A task object.</returns>
        public static Task Enqueue<TResult, TParam>(this TaskQueue<TResult> taskQueue, Action<TParam> action, TParam param)
        {
            return taskQueue.Enqueue(state =>
            {
                action((TParam) state);
                return default;
            }, param);
        }
    }
}