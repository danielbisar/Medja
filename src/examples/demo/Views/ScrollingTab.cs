using System;
using System.Timers;
using Medja.Controls;
using Medja.Properties;
using Medja.Theming;
using Medja.Utils.Threading.Tasks;

namespace Medja.Demo
{
    public class ScrollingTab : ContentControl
    {
        private readonly TextBlock _textBlock;
        private readonly ScrollableContainer _scrollableContainer;
        private readonly Timer _timer;
        
        public ScrollingTab(IControlFactory controlFactory)
        {
            _textBlock = controlFactory.Create<TextBlock>();
            _textBlock.Text = "line 1\nline 2\nline 3\nline 4\nline 5\nline 6\nline 7\nline 8\nline 9\nline 10\nline 1\nline 2\nline 3\nline 4\nline 5\nline 6\nline 7\nline 8\nline 9\nline 10";
            _textBlock.AutoHeightToContent = true;

            _scrollableContainer = controlFactory.Create<ScrollableContainer>();
            _scrollableContainer.Content = _textBlock;

            Content = _scrollableContainer;

            _timer = new Timer(2000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _textBlock.Position.PropertyHeight.PropertyChanged += OnTextBlockHeightChanged;
        }

        private void OnTextBlockHeightChanged(object sender, PropertyChangedEventArgs e)
        {
            _scrollableContainer.ScrollToBottom();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var taskQueue = MedjaApplication.Instance.MainWindow?.TaskQueue;

            if (taskQueue == null)
                return;

            taskQueue.Enqueue(() =>
            {
                _textBlock.Text += "\n" + DateTime.Now;
            });
        }
    }
}