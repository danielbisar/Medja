using System.Diagnostics;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using SensorFrontend.IPC;

namespace SensorFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using(var client = new Client())
            {
                //client.Send("BLA");
                //client.SendAndReceive("Test1");
            }

            return;

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

            stackPanel.Children.Add(factory.Create<Button>(p =>
            {
                p.Text = "Settings";
                p.InputState.MouseClicked += (s, e) => Debug.WriteLine("asd");
            }));
            stackPanel.Children.Add(factory.Create<Button>(p => p.Text = "2"));
            stackPanel.Children.Add(factory.Create<Button>(p => p.Text = "3"));
            stackPanel.Children.Add(factory.Create<Button>(p => 
            {
                p.Text = "Quit";
                p.InputState.MouseClicked += (s, e) => window.Close();
            }));

            var dockPanel = new DockPanel();
            dockPanel.Add(stackPanel, Dock.Right);

            window.Content = dockPanel;

            return window;
        }
    }
}
