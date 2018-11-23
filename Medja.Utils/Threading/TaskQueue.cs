using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Medja.Utils.Threading
{
    /// <summary>
    /// A simple task queue to allow execution of actions on a specific thread. Basically this works a very simple Dispatcher.
    /// </summary>
    /// <typeparam name="TResult">The result of the actions.</typeparam>
    /// <example>
    /// var taskQueue = new TaskQueue(object)();
    ///  
    /// var thread = new Thread(ConsumeQueue);
    /// thread.Start();
    ///
    /// var task = taskQueue.Enqueue(p => null, null); // this action will be execute on "thread" not on the current one
    /// task.Wait();
    ///
    /// ...
    /// </example>
    public class TaskQueue<TResult>
    {
        private readonly ConcurrentQueue<Task<TResult>> _taskQueue;
        private readonly AutoResetEvent _waitHandle;

        public TaskQueue()
        {
            _taskQueue = new ConcurrentQueue<Task<TResult>>();
            _waitHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// Enqueues the given action onto the queue.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="state">The state object (any param you want to pass)</param>
        /// <returns>The task that was created and enqueued.</returns>
        public Task<TResult> Enqueue(Func<object, TResult> action, object state)
        {
            var task = new Task<TResult>(action, state);
            
            _taskQueue.Enqueue(task);
            
            // notifies the consumer thread that a new item is available
            _waitHandle.Set();

            return task;
        }

        /// <summary>
        /// Waits until a task is enqueued and calls the action (can occur multiple times).
        /// </summary>
        /// <param name="action">The action to perform for each task.</param>
        public void WaitAndHandleTasks(Action<Task<TResult>> action)
        {
            _waitHandle.WaitOne();

            while (_taskQueue.TryDequeue(out var task))
                action(task);
        }
    }
}