using System.Threading;
using Medja.Controls;
using Medja.Theming;
using Medja.Utils;
using Medja.Utils.Threading.Tasks;

namespace MedjaOpenGlTestApp.Tests
{
    public class MultithreadingTest
    {
        private readonly IControlFactory _controlFactory;
        private TextBlock _control;
        private TaskQueueFinder _finder;

        public MultithreadingTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            _control = _controlFactory.Create<TextBlock>();
            _control.Text = "Another thread will update this text.";

            _finder = new TaskQueueFinder(_control);
            
            var thread = new Thread(UpdateControl);
            thread.Start();

            return _control;
        }

        private void UpdateControl()
        {
            // note shutdown is not handled properly
            
            int x = 0;
            
            while (true)
            {
                Thread.Sleep(1000);

                x++;

                // prevent modified closure
                var x1 = x;
                _finder.TaskQueue?.Enqueue(() =>
                                                                    {
                                                                        _control.Text =
                                                                                "Cool update from another thread. X = " +
                                                                                x1;
                                                                    });
            }
        }
    }
}