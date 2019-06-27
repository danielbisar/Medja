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
            var theme = new BlackRedTheme();
            var library = new MedjaOpenTkLibrary(theme);
            var application = MedjaApplication.Create(library);
            
            var mainWindow = application.CreateMainWindow();
            mainWindow.Title = "Graph 2D Demo";
            mainWindow.Content = CreateContent();
            mainWindow.CenterOnScreen(800, 600);
        }

        private Control CreateContent()
        {
            var controlFactory = MedjaApplication.Instance.Library.ControlFactory;

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