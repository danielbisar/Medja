using System;
using Medja;
using Medja.Controls;
using Medja.Controls.Container;
using Medja.Controls.Graph2D;
using Medja.OpenTk;
using Medja.OpenTk.Window;
using Medja.OpenTk.Themes.BlackRed;

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
            var controlFactory = new ThemeBlackRed(settings);
            settings.ControlFactory = controlFactory;
            
            MedjaApplication.Create(new MedjaOpenTkLibrary());
            
            var mainWindow = controlFactory.Create<Window>();
            mainWindow.Title = "Graph 2D Demo";
            mainWindow.Content = CreateContent(mainWindow);
            mainWindow.CenterOnScreen(800, 600);
            MedjaApplication.Instance.MainWindow = mainWindow;
        }

        private Control CreateContent(Window window)
        {
            var controlFactory = window.ControlFactory;
            var graph = controlFactory.Create<Graph2D>();
            var plot = controlFactory.Create<Control>();
            plot.Renderer = new SyntheticPlotRenderer(plot);
            // The library's Graph2D series renderer is unfinished; keep plotting local to this example.
            graph.Content = plot;
            
            // just so that we have any container
            var tabControl = controlFactory.Create<TabControl>();
            tabControl.Padding.SetAll(10);
            tabControl.AddTab("Sine wave: 0 to 4 pi, amplitude 1", graph);

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