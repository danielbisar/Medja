using System;

namespace editor
{
    class Program
    {
        static void Main(string[] args)
        {
            var library = new MedjaOpenTkLibrary(new TestAppTheme());
            library.RendererFactory = CreateRenderer;

            var controlFactory = library.ControlFactory;
            var application = MedjaApplication.Create(library);

            _window = application.CreateWindow();
            _window.CenterOnScreen(800, 600);
            _window.Background = Colors.Black;
            _window.Content = test.Create();
            _window.Title = "Editor";

            application.MainWindow = _window;
            application.Run();
        }

        private static IRenderer CreateRenderer()
        {
            var openTkRenderer = new OpenTkRenderer();
            return openTkRenderer;
        }
    }
}
