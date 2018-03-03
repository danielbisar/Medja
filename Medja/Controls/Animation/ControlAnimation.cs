using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Controls.Animation
{
    public class ControlAnimation
    {
        /// <summary>
        /// The animations start time. Will be resetted for revert.
        /// </summary>
        protected long _startTime;
        protected float _precentage;

        public long Duration { get; protected set; }
        public long IsRunningDuration { get; set; }

        public bool IsRunning { get; protected set; }

        public bool IsReverting { get; protected set; }

        protected ControlAnimation()
        {
        }

        internal virtual void Start()
        {
            IsRunning = true;
            _startTime = GetTicks();
        }

        protected long GetTicks()
        {
            return Environment.TickCount;
        }

        internal virtual void Revert()
        {
            IsRunning = true;
            IsReverting = true;
            _startTime = GetTicks();
        }

        internal virtual void Apply()
        {
            IsRunningDuration = GetTicks() - _startTime;
            _precentage = (float)(IsRunningDuration / (double)Duration);
            _precentage = Math.Max(_precentage, 1);

            if (MedjaMath.AboutEquals(_precentage, 1))
                Stop();
        }

        private void Stop()
        {
            IsRunning = false;
            IsReverting = false;
        }
    }
}
