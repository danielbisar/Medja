using Medja;
using Medja.Controls;
using Medja.OpenTk;

namespace SensorFrontend
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = MedjaApplication.Create(new MedjaOpenTkLibrary());
            app.MainWindow = SetupWindow(app.CreateWindow());
            app.Run();
        }

        private static MedjaWindow SetupWindow(MedjaWindow window)
        {
            var position = window.Position;
            position.Width = 800;
            position.Height = 600;

            window.Title = "SensorFrontend";
            window.Content = new DockLayout();

            return window;
        }
    }
}
