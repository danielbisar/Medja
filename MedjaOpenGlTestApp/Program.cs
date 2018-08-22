using Medja;
using Medja.Controls;
using Medja.OpenTk;
using Medja.OpenTk.Rendering;
using OpenTK.Graphics.OpenGL;
using Medja.Primitives;
using System;
using Medja.Performance;

namespace MedjaOpenGlTestApp
{
    class MainClass
    {
        private static MedjaWindow _window;
        private static FramesPerSecondCounter _fpsCounter;

        public static void Main(string[] args)
        {
            _fpsCounter = new FramesPerSecondCounter();
            _fpsCounter.FramesCounted += (s, e) => Console.WriteLine(_fpsCounter.FramesPerSecond);

            var library = new MedjaOpenTkLibrary(new TestAppTheme());
            library.RendererFactory = CreateRenderer;

            var controlFactory = library.ControlFactory;
            var application = MedjaApplication.Create(library);

            var textBlock1 = controlFactory.Create<TextBlock>();
            textBlock1.Position.Height = 50;
            textBlock1.Text = "TextBlock 1";
            textBlock1.Background = new Color(1, 0.2f, 0);

            var textBlock2 = controlFactory.Create<TextBlock>();
            textBlock2.Position.Height = 50;
            textBlock2.Position.Width = 200;
            textBlock2.Text = "TextBlock 2";
            textBlock2.Background = new Color(0, 1, 0);
            textBlock2.HorizontalAlignment = HorizontalAlignment.Left;

            var textBlock3 = controlFactory.Create<TextBlock>();
            textBlock3.Position.Height = 50;
            textBlock3.Text = "TextBlock 3";
            textBlock3.Background = new Color(0, 1, 1);

            var dockPanel = controlFactory.Create<DockPanel>();
            dockPanel.Add(Dock.Top, textBlock1);
            dockPanel.Add(Dock.Bottom, textBlock2);
            dockPanel.Add(Dock.Fill, textBlock3);
            dockPanel.VerticalAlignment = VerticalAlignment.Top;
            dockPanel.Position.Height = 200;

            _window = application.CreateWindow();
            _window.CenterOnScreen(800, 600);
            _window.Background = Colors.Black;
            _window.Content = dockPanel;

            application.MainWindow = _window;
            application.Run();
        }

        private static IRenderer CreateRenderer()
        {
            var openTkRenderer = new OpenTkRenderer();
            openTkRenderer.Before3D = SetupOpenGl;

            return openTkRenderer;
        }

        private static void SetupOpenGl()
        {
            _fpsCounter.Tick();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);

            var color = _window.Background;
            GL.ClearColor(color.Red, color.Green, color.Blue, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }
    }
}
