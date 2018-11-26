using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Medja.Utils.Collections.Concurrent;

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
    public class TaskQueue<TResult> : IDisposable
    {
        private static void DisposeIfCompleted(Task task)
        {
            if(task.IsCompleted)
                task.Dispose();
        }
        
        private readonly ConcurrentQueue<Task<TResult>> _taskQueue;
        private readonly AutoResetEvent _waitHandle;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private int _isDisposed;

        public bool IsDisposed
        {
            get { return _isDisposed == 1; }
        }

        public TaskQueue()
        {
            _taskQueue = new ConcurrentQueue<Task<TResult>>();
            _waitHandle = new AutoResetEvent(false);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Enqueues the given function onto the queue.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <param name="state">The state object (any param you want to pass)</param>
        /// <returns>The task that was created and enqueued.</returns>
        public Task<TResult> Enqueue(Func<object, TResult> func, object state)
        {
            AssureNotDisposed();
            
            var task = new Task<TResult>(func, state, _cancellationTokenSource.Token);
            
            _taskQueue.Enqueue(task);
            
            // notifies the consumer thread that a new item is available
            _waitHandle.Set();

            return task;
        }
        
        /// <summary>
        /// Enqueues the given function onto the queue.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <returns>The functions result.</returns>
        public Task<TResult> Enqueue(Func<TResult> func)
        {
            return Enqueue(state => func(), null);
        }

        private void AssureNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(TaskQueue<TResult>));
        }

        /// <summary>
        /// Waits until a task is enqueued and executes it on the current thread. This method executes the task
        /// synchronously.
        /// </summary>
        public void WaitAndExecuteAll()
        {
            AssureNotDisposed();
            
            _waitHandle.WaitOne();

            while (_isDisposed != 1
                    && !_cancellationTokenSource.IsCancellationRequested
                    && _taskQueue.TryDequeue(out var task))
            {
                task.RunSynchronously();
                DisposeIfCompleted(task);
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                
                _taskQueue.TryDequeueAll(DisposeIfCompleted);
                _waitHandle.Dispose();
                _taskQueue.TryDequeueAll(DisposeIfCompleted);
            }
        }
    }
}