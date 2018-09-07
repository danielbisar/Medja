using System;
using System.Diagnostics;

namespace Medja.Performance
{
    public class FramesPerSecondCounter
    {
        private readonly Stopwatch _stopWatch;
        private int _frameCount;        
        private long _countTicks;

        public float FramesPerSecond {get; private set;}

        public event EventHandler FramesCounted;
        
        public FramesPerSecondCounter()
        {
            _stopWatch = new Stopwatch();
            _countTicks = new TimeSpan(0,0,1).Ticks;
        }

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

            if(FramesCounted != null)
                FramesCounted(this, EventArgs.Empty);
        }
    }
}
