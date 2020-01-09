using System;
using System.IO;
using System.Reflection;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;

namespace Medja.examples.Editor
{
    class Program
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                
                return Path.GetDirectoryName(path);
            }
        }

        static void Main(string[] args)
        {
            var settings = new MedjaOpenTKWindowSettings();
            var controlFactory = new ThemeDarkBlue(settings);
            settings.ControlFactory = controlFactory;
            
            var library = new MedjaOpenTkLibrary();
            var application = MedjaApplication.Create(library);

            var window = controlFactory.Create<Window>();
            window.CenterOnScreen(800, 600);
            window.Background = Colors.Black;
            window.Content = CreateWindowContent(window);
            window.Title = "Editor";

            application.MainWindow = window;
            application.Run();
        }

        private static Control CreateWindowContent(Window window)
        {
            //TODO menu
            var controlFactory = window.ControlFactory;
            var editor = controlFactory.Create<TextEditor>();

            var path = Path.Combine(AssemblyDirectory, "dummy.txt");

            if (File.Exists(path))
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

            window.FocusManager.SetFocus(editor);

            return dockPanel;
        }
    }
}
