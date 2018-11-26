using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Medja.Utils.Threading;
using Xunit;

namespace Medja.Utils.Test.Threading
{
    public class TaskQueueTest
    {
        [Fact]
        public void DoesNotDisposeUnstartedTasks()
        {
            var taskQueue = new TaskQueue<bool>();
            taskQueue.Enqueue(state => true, null);
            taskQueue.Dispose(); // on failure this would throw an exception
        }

        [Fact]
        public void DoesDisposeTasks()
        {
            var taskQueue = new TaskQueue<bool>();
            var task = taskQueue.Enqueue(state => true, null);
            taskQueue.WaitAndExecuteAll();
            taskQueue.Dispose();

            AssertIsTaskDisposed(task);
        }
        
        private void AssertIsTaskDisposed(Task<bool> task)
        {
            Assert.Throws<ObjectDisposedException>(() => ((IAsyncResult) task).AsyncWaitHandle);
        }

        [Fact]
        public void DoesNotExecuteTaskAfterDispose()
        {
            var taskQueue = new TaskQueue<bool>();
            taskQueue.Enqueue(state => true, null);
            taskQueue.Dispose();

            Assert.Throws<ObjectDisposedException>(() => taskQueue.WaitAndExecuteAll());
        }

        [Fact]
        public void ExecuteTasksOnCorrectThread()
        {
            var tasksThreadId = 0;
            
            var taskQueue = new TaskQueue<bool>();
            taskQueue.Enqueue(state =>
            {
                tasksThreadId = Thread.CurrentThread.ManagedThreadId;
                return true;
            }, null);
            
            var thread = new Thread(ExecuteTasks);
            thread.Start(taskQueue);
            thread.Join(TimeSpan.FromMilliseconds(200));
            
            Assert.Equal(tasksThreadId, thread.ManagedThreadId);
        }

        private void ExecuteTasks(object state)
        {
            var queue = (TaskQueue<bool>) state;
            
            while (!queue.IsDisposed)
                queue.WaitAndExecuteAll();
        }

        [Fact]
        public void CanEnqueueFromMultipleThreads()
        {
            var taskQueue = new TaskQueue<bool>();
            var enqueueThreads = new List<Thread>();
            var tasks = new ConcurrentBag<Task<bool>>();

            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() =>
                {
                    for (int n = 0; n < 10; n++)
                    {
                        var task = taskQueue.Enqueue(() =>
                        {
                            tasks.Add(taskQueue.Enqueue(() => true));
                            return true;
                        });
                        
                        tasks.Add(task);
                    }
                });
                
                enqueueThreads.Add(thread);
            }
            
            var executionThread = new Thread(ExecuteTasks);
            executionThread.Start(taskQueue);
            
            foreach(var enqueueThread in enqueueThreads)
                enqueueThread.Start();

            // wait until they are all done
            foreach (var enqueueThread in enqueueThreads)
                enqueueThread.Join();

            // make sure the last action will dispose the task queue
            taskQueue.Enqueue(() =>
            {
                taskQueue.Dispose();
                return true;
            });
            
            executionThread.Join();

            foreach (var thread in enqueueThreads)
                Assert.False(thread.IsAlive);
            
            Assert.Equal(10*10*2, tasks.Count);

            foreach (var task in tasks)
                AssertIsTaskDisposed(task);
        }
    }
}