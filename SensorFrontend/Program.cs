using System;
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

            var animatedButton = factory.Create<Button>();
            animatedButton.Text = "1";
            var animation = new Medja.Controls.Animation.ColorAnimation(animatedButton.BackgroundProperty, new Medja.Primitives.Color(1, 0, 0), new Medja.Primitives.Color(0, 1, 1), new TimeSpan(0, 0, 1).Ticks);
            animation.IsAutoRevert = true;
            animation.IsAutoRestart = true;
            animatedButton.AnimationManager.Start(animation);

            stackPanel.Children.Add(animatedButton);
            stackPanel.Children.Add(factory.Create<Button>(p => p.Text = "2"));
            stackPanel.Children.Add(factory.Create<Button>(p => p.Text = "3"));
            stackPanel.Children.Add(factory.Create<Button>(p => p.Text = "4"));

            var dockPanel = new DockPanel();
            dockPanel.Add(stackPanel, Dock.Right);

            window.Content = dockPanel;

            return window;
        }
    }
}
