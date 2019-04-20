using System;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using Medja.Primitives;
using Medja.Theming;

namespace Medja.examples.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            var library = new MedjaOpenTkLibrary(new Medja.OpenTk.Themes.DarkBlue.DarkBlueTheme());
            library.RendererFactory = CreateRenderer;

            var controlFactory = library.ControlFactory;
            var application = MedjaApplication.Create(library);

            var window = application.CreateWindow();
            window.CenterOnScreen(800, 600);
            window.Background = Colors.Black;
            window.Content = CreateWindowContent(controlFactory);
            window.Title = "Editor";

            application.MainWindow = window;
            application.Run();
        }

        private static Control CreateWindowContent(IControlFactory controlFactory)
        {
            return controlFactory.Create<TextEditor>();
        }

        private static IRenderer CreateRenderer()
        {
            var openTkRenderer = new OpenTkRenderer();
            return openTkRenderer;
        }
    }
}
