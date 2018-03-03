using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medja.Controls.Animation
{
    /// <summary>
    /// Responsible for managing animations for a control.
    /// </summary>
    public class AnimationManager
    {
        private readonly List<ControlAnimation> _animations;

        public AnimationManager()
        {
            _animations = new List<ControlAnimation>();
        }

        public void Start(ControlAnimation animation)
        {
            AddControlNewAnimation(animation);
            animation.Start();
        }

        public void Revert(ControlAnimation animation)
        {
            AddControlNewAnimation(animation);
            animation.Revert();
        }

        private void AddControlNewAnimation(ControlAnimation animation)
        {
            // TODO what happends with multiple animations on the same property?

            // add is a little slower than with a hashset, but iteration faster
            if (_animations.Contains(animation))
                return;

            _animations.Add(animation);
        }

        internal void ApplyAnimations()
        {
            foreach (var animation in _animations.Where(p => p.IsRunning))
                animation.Apply();

            _animations.RemoveAll(p => !p.IsRunning);
        }
    }
}
