using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Themes;

namespace graph2d
{
    class Program : IDisposable
    {
        public static void Main()
        {
            using(var program = new Program())
                program.Run();
        }

        public Program()
        {
            var settings = new MedjaOpenTKWindowSettings();
            var controlFactory = new BlackRedTheme(settings);
            settings.ControlFactory = controlFactory;
            
            MedjaApplication.Create(new MedjaOpenTkLibrary());
            
            var mainWindow = controlFactory.Create<Window>();
            mainWindow.Title = "Graph 2D Demo";
            mainWindow.Content = CreateContent(mainWindow);
            mainWindow.CenterOnScreen(800, 600);
        }

        private Control CreateContent(Window window)
        {
            var controlFactory = window.ControlFactory;
            var graph = controlFactory.Create<Graph2D>();
            
            // just so that we have any container
            var tabControl = controlFactory.Create<TabControl>();
            tabControl.Padding.SetAll(10);
            tabControl.AddTab("Tab 1", graph);

            return tabControl;
        }

        private void Run()
        {
            MedjaApplication.Instance.Run();
        }

        public void Dispose()
        {
            MedjaApplication.Instance.Shutdown();
        }
    }
}