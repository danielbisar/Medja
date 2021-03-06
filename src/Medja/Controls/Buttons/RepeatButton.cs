using System.Timers;
using Medja.Properties;
using Medja.Utils;

namespace Medja.Controls.Buttons
{
    /// <summary>
    /// Sends repetitive click events, when the button is hold.
    /// </summary>
    public class RepeatButton : Button
    {
        private readonly Timer _timer;
        private readonly TaskQueueFinder _taskQueueFinder;

        public readonly Property<int> PropertyClickRepeatMilliseconds;
        public int ClickRepeatMilliseconds
        {
            get { return PropertyClickRepeatMilliseconds.Get(); }
            set { PropertyClickRepeatMilliseconds.Set(value); }
        }

        public RepeatButton()
        {
            PropertyClickRepeatMilliseconds = new Property<int>();
            PropertyClickRepeatMilliseconds.SetSilent(200);
            PropertyClickRepeatMilliseconds.PropertyChanged += OnClickRepeatMillisecondsChanged;

            InputState.PropertyIsLeftMouseDown.PropertyChanged += OnIsLeftMouseDownChanged;

            _taskQueueFinder = new TaskQueueFinder(this);
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerElapsed;
        }

        private void OnClickRepeatMillisecondsChanged(object sender, PropertyChangedEventArgs e)
        {
            _timer.Interval = (int)e.NewValue;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _taskQueueFinder.TaskQueue?.Enqueue(control =>
                {
                    var button = (RepeatButton)control;
                    var position = button.InputState.PointerPosition;
                    button.InputState.SendClick(position);

                    return null;
                }, this);
        }

        protected virtual void OnIsLeftMouseDownChanged(object sender, PropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
                _timer.Start();
            else
                _timer.Stop();
        }
    }
}
