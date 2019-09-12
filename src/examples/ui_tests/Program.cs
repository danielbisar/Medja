using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Themes.DarkBlue;
using Medja.Primitives;
using MedjaOpenGlTestApp.Tests;

namespace MedjaOpenGlTestApp
{
    public static class MainClass
    {
        private static Window _window;

        public static void Main(string[] args)
        {
            var settings = new MedjaOpenTKWindowSettings();
            var theme = new DarkBlueTheme(settings);
            settings.ControlFactory = theme;
            
            var library = new MedjaOpenTkLibrary();
            var application = MedjaApplication.Create(library);

            var test = new ButtonTest(theme);
            //var test = new ButtonRendererPerformance(theme);
            //var test = new ComboBoxTest(theme);
            //var test = new ContentControlTest(theme);
            //var test = new Control3DTest(theme);
            //var test = new DialogTest(theme);
            //var test = new DockPanelTest(theme);
            //var test = new Graph2DTest(theme);
            //var test = new ImageButtonTest(theme);
            //var test = new MultithreadingTest(theme);
            //var test = new NumericKeypadTest(theme);
            //var test = new ScrollableContainerTest(theme);
            //var test = new ScrollingGridTest(theme);
            //var test = new SideControlsContainerTest(theme);
            //var test = new SimpleDockPanelTest(theme);
            //var test = new SliderTest(theme);
            //var test = new TabControlTest(theme);
            //var test = new TextBoxTest(theme);
            //var test = new TouchItemListTest(theme);
            //var test = new VerticalStackPanelTest(theme);
            //var test = new VisibilityTest(theme);

            _window = theme.Create<Window>();
            _window.CenterOnScreen(800, 600);
            _window.Background = Colors.Black;
            _window.Content = test.Create();
            _window.Title = "Demo";

            application.MainWindow = _window;
            application.Run();
        }
    }
}
