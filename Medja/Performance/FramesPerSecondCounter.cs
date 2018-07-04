using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
        
        FramesPerSecondCounter()
        {
            _stopWatch = new Stopwatch();
            _countTicks = new TimeSpan(0,0,1).Ticks;
        }

        public void Tick()
        {
            if(!_stopWatch.IsRunning)
                _stopWatch.Start();

            _frameCount++;

            if(_countTicks < _stopWatch.ElapsedTicks)
                NotifyFramesCounted();
        }

        private void NotifyFramesCounted()
        {
            FramesPerSecond = _frameCount / (float)(_stopWatch.ElapsedMilliseconds / 1000);

            if(FramesCounted != null)
                FramesCounted(this, EventArgs.Empty);
        }
    }
}
