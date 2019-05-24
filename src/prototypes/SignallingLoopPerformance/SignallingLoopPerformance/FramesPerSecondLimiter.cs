using System;
using System.Threading;

namespace SignallingLoopPerformance
{
    public class FramesPerSecondLimiter : IDisposable
    {
        private readonly int _sleepMilliseconds;
        private readonly Thread _timingThread;
        private readonly AutoResetEvent _waitHandle;
        private readonly Action _renderAction;

        private int _isDisposed;
        
        public FramesPerSecondLimiter(int maxFramesPerSecond, Action renderAction)
        {
            _isDisposed = 0;
            _timingThread = new Thread(TimingLoop);
            _sleepMilliseconds = (int)(1000.0f / maxFramesPerSecond);
            _waitHandle = new AutoResetEvent(false);

            _renderAction = renderAction ?? throw new ArgumentNullException(nameof(renderAction));
        }

        public void Run()
        {
            _timingThread.Start();
        }

        public void Join()
        {
            _timingThread.Join();
        }

        private void TimingLoop()
        {
            // how to prevent caching of _isDisposed for this thread?
            // is this even an issue?
            while (_isDisposed != 1)
            {
                _waitHandle.Set();
                
                // even if this method could be very inexact this seems to work just fine for fps limits
                // around 60 fps, and has very low cpu load
                Thread.Sleep(_sleepMilliseconds);
            }
        }

        public void Render()
        {
            _waitHandle.WaitOne();
            _renderAction();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                _waitHandle.Set();
                _waitHandle.Dispose();
            }
        }
    }
}