using Medja;
using Medja.Controls;
using Medja.OpenTk;

namespace SensorFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lib = new MedjaOpenTkLibrary();
            var app = MedjaApplication.Create(lib);
            app.MainWindow = SetupWindow(app.CreateWindow(), lib);
            app.Run();
        }

        private static MedjaWindow SetupWindow(MedjaWindow window, IMedjaLibrary library)
        {
            var factory = library.ControlFactory;

            var position = window.Position;
            position.Width = 800;
            position.Height = 600;

            window.Title = "SensorFrontend";

            var stackPanel = new VerticalStackPanel();
            stackPanel.Padding = new Medja.Primitives.Thickness(10);
            stackPanel.Position.Width = 170;
            stackPanel.ChildrenHeight = 50;
            stackPanel.Children.Add(factory.Create<Button>());
            stackPanel.Children.Add(factory.Create<Button>());
            stackPanel.Children.Add(factory.Create<Button>());
            stackPanel.Children.Add(factory.Create<Button>());

            var dockPanel = new DockPanel();
            dockPanel.Add(stackPanel, Dock.Right);

            window.Content = dockPanel;

            return window;
        }
    }
}
