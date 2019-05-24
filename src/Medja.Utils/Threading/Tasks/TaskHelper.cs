using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Medja.Utils.Linq;

namespace Medja.Utils.Threading.Tasks
{
    /// <summary>
    /// A class providing extension methods or utility methods for TPL.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Creates an IEnumerable of tasks with the same delegate.
        /// </summary>
        /// <param name="count">The count of tasks to create.</param>
        /// <param name="action">The delegate to pass to the task constructor.</param>
        /// <param name="tokenSource">The <see cref="CancellationTokenSource"/> to use.</param>
        /// <param name="creationOptions">The <see cref="TaskCreationOptions"/> to use.</param>
        /// <returns>An <see cref="IEnumerable{Task}"/> which creates tasks only on iteration.</returns>
        public static IEnumerable<Task> CreateN(int count, Action action, CancellationTokenSource tokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            for(int i = 0; i < count; i++)
                yield return new Task(action, tokenSource?.Token ?? CancellationToken.None, creationOptions);
        }

        /// <summary>
        /// Creates a list with <see cref="count"/> tasks.
        /// </summary>
        /// <param name="count">The count of tasks to create.</param>
        /// <param name="action">The delegate to pass to the task constructor.</param>
        /// <param name="tokenSource">The <see cref="CancellationTokenSource"/> to use.</param>
        /// <param name="creationOptions">The <see cref="TaskCreationOptions"/> to use.</param>
        /// <returns>A list of tasks with <see cref="List{T}.Count"/> equals <see cref="count"/>.</returns>
        public static IList<Task> CreateNAsList(int count, Action action, CancellationTokenSource tokenSource = null, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            var result = new List<Task>(count);
            result.AddRange(CreateN(count, action, tokenSource, creationOptions));

            return result;
        }

        /// <summary>
        /// Calls start on all tasks returned by <see cref="IEnumerable{Task}"/>.
        /// </summary>
        /// <param name="tasks">The tasks you want to start.</param>
        /// <param name="taskScheduler">The <see cref="TaskScheduler"/> to use. Default = <see cref="TaskScheduler.Current"/>.</param>
        public static void StartAll(this IEnumerable<Task> tasks, TaskScheduler taskScheduler = null)
        {
            foreach (var task in tasks)
                task.Start(taskScheduler ?? TaskScheduler.Current);
        }

        /// <summary>
        /// Basically an overload for <see cref="Task.WaitAll(System.Threading.Tasks.Task[])"/>.
        /// </summary>
        /// <param name="tasks">The tasks to wait for.</param>
        /// <param name="timeout">If null we use <see cref="Timeout.InfiniteTimeSpan"/>.</param>
        public static void WaitAll(this IEnumerable<Task> tasks, TimeSpan? timeout = null)
        {
            Task.WaitAll(tasks.AssureIsArray(), timeout ?? Timeout.InfiniteTimeSpan);
        }
    }
}