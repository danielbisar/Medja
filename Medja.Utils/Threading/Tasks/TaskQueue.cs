using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Medja.Utils.Collections.Concurrent;

namespace Medja.Utils.Threading.Tasks
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

        /// <summary>
        /// A cancellation token source that should be used if you are waiting for any task the is enqueue here.
        ///
        /// task.Wait(taskQueue.CancellationTokenSource.Token) else not executed tasks will wait forever.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
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

            ExecuteAll();
        }

        /// <summary>
        /// Executes all tasks currently placed inside the queue.
        /// </summary>
        public void ExecuteAll()
        {
            AssureNotDisposed();
            
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
                
                // it seems that mono has a bug here, so we need to call _waitHandle.Set before calling
                // Dispose, else _waitHandle would still block forever
                _waitHandle.Set();
                
                _waitHandle.Dispose();
                _taskQueue.TryDequeueAll(DisposeIfCompleted);
            }
        }
    }
}