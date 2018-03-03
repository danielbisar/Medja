using System;
using System.Collections.Generic;
using System.Text;

namespace Medja.Controls.Animation
{
    /// <summary>
    /// Responsible for managing animations for a control.
    /// </summary>
    public class AnimationManager
    {
        private readonly Control _control;
        private readonly HashSet<ControlAnimation> _animations;

        public AnimationManager(Control control)
        {
            _control = control ?? throw new ArgumentNullException(nameof(control));
            _animations = new HashSet<ControlAnimation>();
        }

        public void Start(ControlAnimation animation)
        {
            _animations.Add(animation);
            animation.Start();
        }

        public void Revert(ControlAnimation animation)
        {
            _animations.Add(animation);
            animation.Revert();
        }

        internal void ApplyAnimations()
        {
            foreach (var animation in _animations)
                animation.Apply();
        }
    }
}
