using System;
using System.IO;
using System.Reflection;
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
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

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
            //TODO menu

            var editor = controlFactory.Create<TextEditor>();

			var path = Path.Combine(AssemblyDirectory, "dummy.txt");

            if(File.Exists(path))
                editor.SetText(File.ReadAllText(path));

            var btn = controlFactory.Create<Button>();
            btn.Text = "Du";

            var buttonStackPanel = controlFactory.Create<HorizontalStackPanel>();
            buttonStackPanel.ChildrenWidth = 60;
            buttonStackPanel.Position.Height = btn.Position.Height;
            buttonStackPanel.Background = editor.Background;
            buttonStackPanel.Children.Add(btn);

            var dockPanel = controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Bottom, buttonStackPanel);
            dockPanel.Add(Dock.Fill, editor);

            FocusManager.Default.SetFocus(editor);

            return dockPanel;
        }

        private static IRenderer CreateRenderer()
        {
            var openTkRenderer = new OpenTkRenderer();
            return openTkRenderer;
        }
    }
}
