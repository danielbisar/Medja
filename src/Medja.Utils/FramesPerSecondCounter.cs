using System;
using System.Diagnostics;

namespace Medja.Utils
{
    /// <summary>
    /// A class that counts the frames rendered per second.
    /// </summary>
    public class FramesPerSecondCounter
    {
        private readonly Stopwatch _stopWatch;
        private readonly long _countTicks;
        
        private int _frameCount;

        /// <summary>
        /// The last counted FPS value.
        /// </summary>
        public float FramesPerSecond {get; private set;}

        /// <summary>
        /// Event that triggers when a new FPS value is calculated. (About every second)
        /// </summary>
        public event EventHandler<ValueEventArgs<float>> FramesCounted;
        
        public FramesPerSecondCounter(int countSeconds = 1)
        {
            _stopWatch = new Stopwatch();
            _countTicks = new TimeSpan(0,0, countSeconds).Ticks;
        }

        /// <summary>
        /// Call this method in your render loop.
        /// </summary>
        public void Tick()
        {
            if(!_stopWatch.IsRunning)
                _stopWatch.Start();

            _frameCount++;

            if (_countTicks < _stopWatch.ElapsedTicks)
            {
                NotifyFramesCounted();
                _stopWatch.Restart();
                _frameCount = 0;
            }
        }

        private void NotifyFramesCounted()
        {
            FramesPerSecond = _frameCount / (float)(_stopWatch.ElapsedMilliseconds / 1000.0);
            FramesCounted?.Invoke(this, new ValueEventArgs<float>(FramesPerSecond));
        }
    }
}
