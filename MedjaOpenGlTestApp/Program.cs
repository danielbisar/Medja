using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;

namespace MedjaOpenGlTestApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			var library = new MedjaOpenTkLibrary();
			var application = MedjaApplication.Create(library);

			var window = application.CreateWindow();
			application.MainWindow = window;

			window.Position.X = 800;
			window.Position.Y = 600;

			var stackPanel = new VerticalStackPanel();
			stackPanel.Position.Width = 170;
			stackPanel.ChildrenHeight = 50;
			stackPanel.Children.Add(library.ControlFactory.Create<Button>(p => p.Text = "123"));

			window.Content = stackPanel;

			application.Run();
        }
    }
}
