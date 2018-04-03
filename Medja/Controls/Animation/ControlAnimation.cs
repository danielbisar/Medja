using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// If set to true, after reaching the target value the animation reverts.
        /// </summary>
        public bool IsAutoRevert { get; set; }
        public bool IsAutoRestart { get; set; }

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
            return DateTime.UtcNow.Ticks;
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
            _precentage = Math.Min(_precentage, 1);

            if (IsReverting)
                _precentage = 1 - _precentage;

            if (IsAtEnd(_precentage))
            {
                if (IsAutoRevert && !IsReverting)
                    Revert();
                else if (IsAutoRestart)
                    Start();
                else
                    Stop();
            }
        }

        private bool IsAtEnd(float percentage)
        {
            return IsReverting ? MedjaMath.AboutEquals(_precentage, 0) : MedjaMath.AboutEquals(_precentage, 1);
        }

        private void Stop()
        {
            IsRunning = false;
            IsReverting = false;
        }
    }
}
