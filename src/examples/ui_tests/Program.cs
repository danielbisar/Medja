using System;
using System.Collections.Generic;
using System.Diagnostics;
using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using Medja.OpenTk.Themes;
using Medja.OpenTk.Themes.DarkBlue;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using Medja.Theming;
using Medja.Utils;
using MedjaOpenGlTestApp.Tests;
using MedjaOpenGlTestApp.Tests.Performance;
using OpenTK;
using SkiaSharp;

namespace MedjaOpenGlTestApp
{
    public static class MainClass
    {
        private static MedjaWindow _window;

        public static void Main(string[] args)
        {
            var theme = new DarkBlueTheme();
            //var theme = new BlackRedTheme();
            
            var library = new MedjaOpenTkLibrary(theme);
            library.RendererFactory = CreateRenderer;

            var controlFactory = library.ControlFactory;
            var application = MedjaApplication.Create(library);

            //var test = new ButtonTest(controlFactory);
            var test = new ButtonRendererPerformance(controlFactory);
            //var test = new ComboBoxTest(controlFactory);
            //var test = new ContentControlTest(controlFactory);
            //var test = new Control3DTest(controlFactory);
            //var test = new DialogTest(controlFactory);
            //var test = new DockPanelTest(controlFactory);
            //var test = new Graph2DTest(controlFactory);
            //var test = new ImageButtonTest(controlFactory);
            //var test = new MultithreadingTest(controlFactory);
            //var test = new NumericKeypadTest(controlFactory);
            //var test = new ScrollableContainerTest(controlFactory);
            //var test = new ScrollingGridTest(controlFactory);
            //var test = new SideControlsContainerTest(controlFactory);
            //var test = new SimpleDockPanelTest(controlFactory);
            //var test = new SliderTest(controlFactory);
            //var test = new TabControlTest(controlFactory);
            //var test = new TextBoxTest(controlFactory);
            //var test = new TouchItemListTest(controlFactory);
            //var test = new VerticalStackPanelTest(controlFactory);
            //var test = new VisibilityTest(controlFactory);

            _window = application.CreateWindow();
            _window.CenterOnScreen(800, 600);
            _window.Background = Colors.Black;
            _window.Content = test.Create();
            _window.Title = "Demo";

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
