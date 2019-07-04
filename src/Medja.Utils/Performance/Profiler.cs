using System;
using System.Diagnostics;

namespace Medja.Utils.Performance
{
    /// <summary>
    /// Allows profiling of method calls.
    /// </summary>
    public class Profiler
    {
        public static Profiler Create(string className, string methodName)
        {
            return new Profiler(className, methodName);
        }

        // INSTANCE
        
        private readonly string _className;
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;
        
        private int _callCount;

        /// <summary>
        /// Gets the count of calls that was registered with <see cref="Profile"/>.
        /// </summary>
        public int CallCount { get => _callCount; }

        /// <summary>
        /// Gets the elapsed milliseconds for all calls.
        /// </summary>
        public long TotalMilliseconds { get => _stopwatch.ElapsedMilliseconds; }

        /// <summary>
        /// Gets or sets if Reset is called every time <see cref="NotificationCondition"/> is met.
        /// </summary>
        public bool AutoReset { get;set; }
        
        /// <summary>
        /// Gets the average ms a call took.
        /// </summary>
        public float AverageMSPerCall
        {
            get
            {
                if(_callCount == 0)
                    return 0;
                
                return (float)TotalMilliseconds / _callCount;
            }
        }

        /// <summary>
        /// Gets or sets the condition that defines when <see cref="Notify"/> should be raised. If set to null the event
        /// will never occur.
        /// </summary>
        public Func<Profiler, bool> NotificationCondition { get; set; }
        
        /// <summary>
        /// This event occurs when <see cref="NotificationCondition"/> is met.
        /// </summary>
        public event EventHandler Notify;

        private Profiler(string className, string methodName)
        {
            _className = className;
            _methodName = methodName;
            _stopwatch = new Stopwatch();
            _callCount = 0;

            AutoReset = true;
        }

        /// <summary>
        /// Profiles the method call.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">If <see cref="action"/> == null.</exception>
        public void Profile(Action action)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));
            
            if (NotificationCondition?.Invoke(this) == true)
            {
                RaiseNotify();
                
                if(AutoReset)
                    Reset();
            }

            _callCount++;
            _stopwatch.Start();
            action();
            _stopwatch.Stop();
        }

        /// <summary>
        /// Resets the call counter and time measured.
        /// </summary>
        public void Reset()
        {
            _callCount = 0;
            _stopwatch.Reset();
        }

        /// <summary>
        /// A brief summary about the profiled data.
        /// </summary>
        /// <returns>The summary string.</returns>
        public override string ToString()
        {
            return $"{_className}.{_methodName}: called {_callCount} times - avg ms per call {AverageMSPerCall}";
        }

        protected virtual void RaiseNotify()
        {
            Notify?.Invoke(this, EventArgs.Empty);
        }
    }
}