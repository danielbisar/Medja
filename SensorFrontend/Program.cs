using Medja;
using Medja.Controls;
using Medja.OpenTk;

namespace SensorFrontend
{
    public class Program
    {
        public static void Main(string[] args)
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

            var stackPanel = new VerticalStackPanel();
            stackPanel.Position.Width = 100;
            stackPanel.Children.Add(new Button());
            stackPanel.Children.Add(new Button());
            stackPanel.Children.Add(new Button());
            stackPanel.Children.Add(new Button());

            var dockPanel = new DockPanel();
            dockPanel.Add(stackPanel, Dock.Right);

            window.Content = dockPanel;

            return window;
        }
    }
}
