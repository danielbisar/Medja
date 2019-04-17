using System.Threading;
using Medja;
using Medja.Controls;
using Medja.Theming;
using Medja.Utils.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MedjaOpenGlTestApp.Tests
{
    public class MultithreadingTest
    {
        private readonly IControlFactory _controlFactory;
        private TextBlock _control;

        public MultithreadingTest(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
        }

        public Control Create()
        {
            _control = _controlFactory.Create<TextBlock>();
            _control.Text = "Another thread will update this text.";
            
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
                MedjaApplication.Instance.Library.TaskQueue.Enqueue(() =>
                                                                    {
                                                                        _control.Text =
                                                                                "Cool update from another thread. X = " +
                                                                                x1;
                                                                    });
            }
        }
    }
}